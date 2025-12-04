namespace LeadsSaas.Api.Integrations.Adapters
{
    public interface ILeadSourceAdapter
    {
        string SourceName { get; } // GoogleAds, FacebookAds, CsvImport, etc.

        Task<IReadOnlyList<UnifiedLead>> GetLeadsAsync(DateTime from, DateTime to, CancellationToken ct = default);
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
