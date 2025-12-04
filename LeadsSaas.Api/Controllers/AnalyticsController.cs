using LeadsSaas.Api.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("tenants/{tenantId:guid}/analytics/leads")]
public class AnalyticsController : ControllerBase
{
    [HttpGet("summary")]
    public ActionResult<LeadAnalyticsSummaryResponse> GetSummary(Guid tenantId)
    {
        // En la PoC devolvemos datos fake / hardcode.
        var today = DateTime.UtcNow.Date;
        var last30Days = Enumerable.Range(0, 30)
            .Select(offset => new LeadsPerDayDto
            {
                Date = today.AddDays(-offset),
                Count = 50 + offset   // solo para demo
            })
            .OrderBy(x => x.Date)
            .ToList();

        var response = new LeadAnalyticsSummaryResponse
        {
            LeadsPerDay = last30Days,
            Funnel = new FunnelDto
            {
                Visitors = 10000,
                Leads = 3000,
                Mql = 1500,
                Sql = 600,
                Customers = 200
            },
            CostPerLead = 2.35m
        };

        return Ok(response);
    }
}
