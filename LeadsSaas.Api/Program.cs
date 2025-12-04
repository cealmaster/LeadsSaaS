using System.Text;
using LeadsSaas.Api.Infrastructure.MultiTenancy;
using LeadsSaas.Api.Infrastructure.Persistence;
using LeadsSaas.Api.Integrations.Adapters;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Clave JWT fija solo para DEMO (mínimo 32 caracteres)
// En producción: mover a Secret Manager / Vault.
const string JwtKey = "this_is_a_demo_jwt_key_with_32_chars";

var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtKey));

// ================= JWT AUTH =================
builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateIssuerSigningKey = false,
            ValidateLifetime = false,
            ClockSkew = TimeSpan.FromMinutes(2),

            ValidIssuer = "LeadsSaas",
            ValidAudience = "LeadsSaasClients",
            IssuerSigningKey = signingKey
        };
    });

builder.Services.AddAuthorization();

// ================= MULTI-TENANCY =================
builder.Services.AddScoped<ITenantProvider, TenantProvider>();

// DbContext por tenant (SQL Server)
builder.Services.AddDbContext<TenantDbContext>();

// ================= HTTP CLIENTS =================
builder.Services.AddHttpClient("ExternalIpClient", client =>
{
    client.BaseAddress = new Uri("https://api.ipify.org");
});

// ================= INTEGRATIONS =================
builder.Services.AddScoped<ILeadSourceAdapterFactory, LeadSourceAdapterFactory>();
builder.Services.AddScoped<GoogleAdsLeadAdapter>();
builder.Services.AddScoped<FacebookAdsLeadAdapter>();

// ================= MVC =================
builder.Services.AddControllers();

// ================= SWAGGER =================
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Leads SaaS API",
        Version = "v1",
        Description = "API multi-tenant para gestión y analytics de leads"
    });

    // Definimos el esquema Bearer
    var jwtSecurityScheme = new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Description = "Introduce SOLO el token JWT, sin 'Bearer ' ni comillas.",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        Reference = new OpenApiReference
        {
            Type = ReferenceType.SecurityScheme,
            Id = "Bearer"
        }
    };

    c.AddSecurityDefinition("Bearer", jwtSecurityScheme);

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        { jwtSecurityScheme, Array.Empty<string>() }
    });
});

var app = builder.Build();

// ================= PIPELINE =================
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Leads SaaS API v1");
        c.DocumentTitle = "Leads SaaS API";
    });
}

app.UseAuthentication();
app.UseMiddleware<TenantMiddleware>(); // debe ir antes de UseAuthorization
app.UseAuthorization();

app.MapControllers();

app.Run();
