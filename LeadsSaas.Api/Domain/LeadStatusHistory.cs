namespace LeadsSaas.Api.Domain
{
    public class LeadStatusHistory
    {
        public Guid Id { get; set; }
        public Guid LeadId { get; set; }
        public string OldStatus { get; set; } = default!;
        public string NewStatus { get; set; } = default!;
        public DateTime ChangedAt { get; set; } = DateTime.UtcNow;
        public string ChangedBy { get; set; } = default!; // userId / sistema
    }
}
