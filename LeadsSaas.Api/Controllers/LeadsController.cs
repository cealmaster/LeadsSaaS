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
        var query = _db.Leads.AsNoTracking();

        var total = await query.CountAsync();
        var items = await query
            .OrderByDescending(l => l.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(l => new LeadDto
            {
                Id = l.Id,
                FullName = l.FullName,
                Email = l.Email,
                Status = l.Status,
                Source = l.Source,
                CreatedAt = l.CreatedAt
            })
            .ToListAsync();

        return Ok(new PagedResult<LeadDto>(items, total, page, pageSize));
    }

    [HttpPatch("{leadId:guid}")]
    public async Task<ActionResult<LeadDto>> UpdateLead(Guid tenantId, Guid leadId, [FromBody] UpdateLeadRequest request)
    {
        var lead = await _db.Leads.FindAsync(leadId);
        if (lead == null) return NotFound();

        var userId = User.Identity?.Name ?? "system";

        // Ejemplo: actualización de estado + auditoría
        if (!string.IsNullOrWhiteSpace(request.Status) && request.Status != lead.Status)
        {
            var oldStatus = lead.Status;
            lead.Status = request.Status;
            lead.UpdatedAt = DateTime.UtcNow;

            _db.LeadStatusHistory.Add(new LeadStatusHistory
            {
                LeadId = lead.Id,
                OldStatus = oldStatus,
                NewStatus = lead.Status,
                ChangedAt = DateTime.UtcNow,
                ChangedBy = userId
            });

            _db.AuditLogs.Add(new AuditLog
            {
                EntityType = "Lead",
                EntityId = lead.Id,
                PropertyName = "Status",
                OldValue = oldStatus,
                NewValue = lead.Status,
                ChangedAt = DateTime.UtcNow,
                ChangedBy = userId,
                Origin = "API"
            });
        }

        // Aquí podrías manejar otros campos (FullName, Email, Phone, etc.)

        await _db.SaveChangesAsync();

        var dto = new LeadDto
        {
            Id = lead.Id,
            FullName = lead.FullName,
            Email = lead.Email,
            Status = lead.Status,
            Source = lead.Source,
            CreatedAt = lead.CreatedAt
        };

        return Ok(dto);
    }
}

