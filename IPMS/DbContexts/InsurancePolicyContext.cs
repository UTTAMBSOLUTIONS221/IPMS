using IPMS.Models;
using Microsoft.EntityFrameworkCore;


namespace IPMS.DbContexts
{
    public class InsurancePolicyContext : DbContext
    {
        public InsurancePolicyContext(DbContextOptions<InsurancePolicyContext> options)
            : base(options)
        {
        }

        public DbSet<InsurancePolicy> Policies { get; set; }
        public DbSet<PolicyHolder> PolicyHolders { get; set; }
    }
}
