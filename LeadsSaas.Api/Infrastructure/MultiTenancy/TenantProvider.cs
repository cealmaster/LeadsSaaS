namespace LeadsSaas.Api.Infrastructure.MultiTenancy
{
    public class TenantProvider : ITenantProvider
    {
        private TenantInfo? _current;

        public TenantInfo Current =>
            _current ?? throw new InvalidOperationException("Tenant no resuelto para la petición actual.");

        public void SetTenant(TenantInfo tenant)
        {
            _current = tenant;
        }
    }
}
