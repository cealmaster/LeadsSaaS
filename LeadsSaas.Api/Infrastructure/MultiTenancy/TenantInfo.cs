namespace LeadsSaas.Api.Infrastructure.MultiTenancy
{
    public class TenantInfo
    {
        public Guid TenantId { get; set; }
        public string Name { get; set; } = default!;
        public string ConnectionString { get; set; } = default!;
    }

    public interface ITenantProvider
    {
        TenantInfo Current { get; }
        void SetTenant(TenantInfo tenant);
    }

}
