using LeadsSaas.Api.Integrations.Adapters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Authorize]
[Route("tenants/{tenantId:guid}/integrations")]
public class IntegrationsController : ControllerBase
{
    private readonly ILeadSourceAdapterFactory _adapterFactory;

    public IntegrationsController(ILeadSourceAdapterFactory adapterFactory)
    {
        _adapterFactory = adapterFactory;
    }

    [HttpGet("{source}/leads")]
    public async Task<ActionResult<IReadOnlyList<UnifiedLead>>> GetLeadsFromSource(
        Guid tenantId,
        string source,
        [FromQuery] DateTime? from = null,
        [FromQuery] DateTime? to = null,
        CancellationToken ct = default)
    {
        var adapter = _adapterFactory.GetAdapter(source);
        var fromDate = from ?? DateTime.UtcNow.AddDays(-7);
        var toDate = to ?? DateTime.UtcNow;

        var leads = await adapter.GetLeadsAsync(fromDate, toDate, ct);
        return Ok(leads);
    }
}
