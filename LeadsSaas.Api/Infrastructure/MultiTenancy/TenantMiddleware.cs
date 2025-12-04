using Microsoft.AspNetCore.Http;

namespace LeadsSaas.Api.Infrastructure.MultiTenancy;

public class TenantMiddleware
{
    private readonly RequestDelegate _next;

    public TenantMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context, ITenantProvider tenantProvider, IConfiguration config)
    {
        var path = context.Request.Path.Value?.ToLowerInvariant() ?? string.Empty;

        // Rutas que NO requieren tenant (login, swagger, health, etc.)
        if (path.StartsWith("/auth") ||
            path.StartsWith("/swagger") ||
            path.StartsWith("/health") ||
            path.StartsWith("/external"))
        {
            await _next(context);
            return;
        }

        string? tenantIdStr = null;

        // 1) Header X-Tenant-Id
        if (context.Request.Headers.TryGetValue("X-Tenant-Id", out var headerValues))
        {
            tenantIdStr = headerValues.FirstOrDefault();
        }

        // 2) Claim "tenant" del JWT
        if (string.IsNullOrWhiteSpace(tenantIdStr))
        {
            tenantIdStr = context.User?.FindFirst("tenant")?.Value;
        }

        // 3) Route value {tenantId} de la URL
        if (string.IsNullOrWhiteSpace(tenantIdStr) &&
            context.Request.RouteValues.TryGetValue("tenantId", out var routeTenantObj))
        {
            tenantIdStr = routeTenantObj?.ToString();
        }

        // Si no tenemos un GUID válido → 400
        if (!Guid.TryParse(tenantIdStr, out var tenantId))
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            await context.Response.WriteAsync($"Tenant inválido o no especificado. Valor recibido: '{tenantIdStr ?? "null"}'");
            return;
        }

        // Obtener connection string por tenant
        var connectionString = config.GetConnectionString($"Tenant_{tenantId}")
                               ?? config.GetConnectionString("DefaultTenant");

        if (string.IsNullOrWhiteSpace(connectionString))
        {
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            await context.Response.WriteAsync("No se encontró connection string para el tenant.");
            return;
        }

        var tenantInfo = new TenantInfo
        {
            TenantId = tenantId,
            Name = $"Tenant-{tenantId}",
            ConnectionString = connectionString
        };

        tenantProvider.SetTenant(tenantInfo);

        await _next(context);
    }
}
