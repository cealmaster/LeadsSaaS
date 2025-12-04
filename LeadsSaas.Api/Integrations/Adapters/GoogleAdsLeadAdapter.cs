namespace LeadsSaas.Api.Integrations.Adapters
{
    public class GoogleAdsLeadAdapter : ILeadSourceAdapter
    {
        public string SourceName => "GoogleAds";

        // Inyectarías un cliente específico de Google Ads; aquí solo simulamos.
        public Task<IReadOnlyList<UnifiedLead>> GetLeadsAsync(DateTime from, DateTime to, CancellationToken ct = default)
        {
            // Llamadas reales a Google Ads API irían aquí.
            var fake = new List<UnifiedLead>
        {
            new UnifiedLead
            {
                ExternalId = "ga-123",
                FullName = "John Doe",
                Email = "john@example.com",
                CampaignName = "Brand Campaign",
                Status = "MQL",
                CreatedAt = DateTime.UtcNow.AddDays(-1)
            }
        };

            return Task.FromResult((IReadOnlyList<UnifiedLead>)fake);
        }
    }

}
