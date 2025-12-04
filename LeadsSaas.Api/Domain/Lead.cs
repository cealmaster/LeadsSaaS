namespace LeadsSaas.Api.Domain
{
    public class Lead
    {
        public Guid Id { get; set; }
        public string FullName { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string? Phone { get; set; }
        public string Source { get; set; } = default!;  // GoogleAds, Facebook, CSV, CRM, etc.
        public string Status { get; set; } = "New";     // New, MQL, SQL, Customer...
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }
}
