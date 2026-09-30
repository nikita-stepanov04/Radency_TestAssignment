using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Radency_TestAssignment.Domain.Entities.Identity;

namespace Radency_TestAssignment.Infrastructure.EFRepositories
{
    public class EFDataContext : IdentityDbContext<User, Role, int>
    {
        public EFDataContext(DbContextOptions<EFDataContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(EFDataContext).Assembly);
        }
    }
}
