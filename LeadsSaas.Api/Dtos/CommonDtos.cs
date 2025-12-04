namespace LeadsSaas.Api.Dtos
{
    public class LoginRequest
    {
        public string Email { get; set; } = default!;
        public string Password { get; set; } = default!;
    }

    public class LoginResponse
    {
        public string Token { get; set; } = default!;
        public int ExpiresIn { get; set; }
        public UserDto User { get; set; } = default!;
    }

    public class UserDto
    {
        public string Email { get; set; } = default!;
    }
    public class LeadDto
    {
        public Guid Id { get; set; }
        public string FullName { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string Status { get; set; } = default!;
        public string Source { get; set; } = default!;
        public DateTime CreatedAt { get; set; }
    }

    public class UpdateLeadRequest
    {
        public string? Status { get; set; }
    }
    public record PagedResult<T>(IReadOnlyList<T> Items, int Total, int Page, int PageSize);
    public class LeadAnalyticsSummaryResponse
    {
        public List<LeadsPerDayDto> LeadsPerDay { get; set; } = new();
        public FunnelDto Funnel { get; set; } = new();
        public decimal CostPerLead { get; set; }
    }

    public class LeadsPerDayDto
    {
        public DateTime Date { get; set; }
        public int Count { get; set; }
    }

    public class FunnelDto
    {
        public int Visitors { get; set; }
        public int Leads { get; set; }
        public int Mql { get; set; }
        public int Sql { get; set; }
        public int Customers { get; set; }
    }
    public class IpifyResponse
    {
        public string Ip { get; set; } = default!;
    }

    public class IpInfoDto
    {
        public string Ip { get; set; } = default!;
    }

    public class UnifiedLead
    {
        public string ExternalId { get; set; } = default!;
        public string FullName { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string? Phone { get; set; }
        public string CampaignName { get; set; } = default!;
        public string Status { get; set; } = "New";
        public DateTime CreatedAt { get; set; }
    }
}
