namespace LeadsSaas.Api.Domain
{
    public class AuditLog
    {
        public Guid Id { get; set; }
        public string EntityType { get; set; } = default!; // ej: "Lead"
        public Guid EntityId { get; set; }
        public string PropertyName { get; set; } = default!;
        public string? OldValue { get; set; }
        public string? NewValue { get; set; }
        public DateTime ChangedAt { get; set; } = DateTime.UtcNow;
        public string ChangedBy { get; set; } = default!;
        public string Origin { get; set; } = default!;     // UI, API, IMPORT
    }
}
