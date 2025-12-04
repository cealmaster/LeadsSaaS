namespace LeadsSaas.Api.Integrations.Adapters
{
    public class FacebookAdsLeadAdapter : ILeadSourceAdapter
    {
        public string SourceName => "FacebookAds";

        public Task<IReadOnlyList<UnifiedLead>> GetLeadsAsync(DateTime from, DateTime to, CancellationToken ct = default)
        {
            var fake = new List<UnifiedLead>
        {
            new UnifiedLead
            {
                ExternalId = "fb-987",
                FullName = "Jane Smith",
                Email = "jane@example.com",
                CampaignName = "Lead Gen Form",
                Status = "New",
                CreatedAt = DateTime.UtcNow.AddDays(-2)
            }
        };

            return Task.FromResult((IReadOnlyList<UnifiedLead>)fake);
        }
    }

}
