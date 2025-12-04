using LeadsSaas.Api.Domain;
using LeadsSaas.Api.Dtos;
using LeadsSaas.Api.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
//[Authorize]
[Route("tenants/{tenantId:guid}/leads")]
public class LeadsController : ControllerBase
{
    private readonly TenantDbContext _db;

    public LeadsController(TenantDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<ActionResult<PagedResult<LeadDto>>> GetLeads(
        Guid tenantId,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20)
    {
        // Datos quemados para demo (sin DB)
        var allLeads = new List<LeadDto>
        {
            new LeadDto
            {
                Id = Guid.Parse("00000000-0000-0000-0000-000000000001"),
                FullName = "Juan Pérez",
                Email = "juan.perez@example.com",
                Status = "New",
                Source = "GoogleAds",
                CreatedAt = DateTime.UtcNow.AddDays(-2)
            },
            new LeadDto
            {
                Id = Guid.Parse("00000000-0000-0000-0000-000000000002"),
                FullName = "María Gómez",
                Email = "maria.gomez@example.com",
                Status = "MQL",
                Source = "FacebookAds",
                CreatedAt = DateTime.UtcNow.AddDays(-1)
            },
            new LeadDto
            {
                Id = Guid.Parse("00000000-0000-0000-0000-000000000003"),
                FullName = "Carlos Rodríguez",
                Email = "carlos.rodiguez@example.com",
                Status = "SQL",
                Source = "Organic",
                CreatedAt = DateTime.UtcNow
            }
        };
        //var query = _db.Leads.AsNoTracking();

        // Paginación en memoria
        var total = allLeads.Count;
        var items = allLeads
            .OrderByDescending(l => l.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        return Ok(new PagedResult<LeadDto>(items, total, page, pageSize));
    }

    [HttpPatch("{leadId:guid}")]
    public async Task<ActionResult<LeadDto>> UpdateLead(Guid tenantId, Guid leadId, [FromBody] UpdateLeadRequest request)
    {
        var originalStatus = "New";

        // Si no viene status, dejamos el original
        var newStatus = string.IsNullOrWhiteSpace(request.Status)
            ? originalStatus
            : request.Status;

        // Construimos un LeadDto "actualizado" de prueba
        var updatedLead = new LeadDto
        {
            Id = leadId,
            FullName = "Lead de prueba",
            Email = "lead.demo@example.com",
            Status = newStatus,
            Source = "Demo",
            CreatedAt = DateTime.UtcNow.AddDays(-1)
        };

        return Ok(updatedLead);
    }
}

