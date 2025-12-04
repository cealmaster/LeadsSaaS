using LeadsSaas.Api.Domain;
using LeadsSaas.Api.Infrastructure.MultiTenancy;
using Microsoft.EntityFrameworkCore;

namespace LeadsSaas.Api.Infrastructure.Persistence
{
    public class TenantDbContext : DbContext
    {
        private readonly ITenantProvider _tenantProvider;

        public TenantDbContext(ITenantProvider tenantProvider)
        {
            _tenantProvider = tenantProvider;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var cs = _tenantProvider.Current.ConnectionString;

            optionsBuilder.UseSqlServer(cs);
        }

        public DbSet<Lead> Leads => Set<Lead>();
        public DbSet<LeadStatusHistory> LeadStatusHistory => Set<LeadStatusHistory>();
        public DbSet<AuditLog> AuditLogs => Set<AuditLog>();

        // Añade aquí el resto de DbSet que necesites:
        // public DbSet<Campaign> Campaigns => Set<Campaign>();
        // public DbSet<LeadCustomField> LeadCustomFields => Set<LeadCustomField>();
        // etc.
    }
}
