using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Radency_TestAssignment.Domain.Entities.Leasing;
using Radency_TestAssignment.Infrastructure.EFRepository;

namespace Radency_TestAssignment.Infrastructure.EFRepositories.Leases
{
    public class EFLeaseOnModelCreating : EFOnModelCreatingBase<Lease>
    {
        protected override void OnModelCreating(EntityTypeBuilder<Lease> model)
        {
            model.HasIndex(e => new { e.UnitID, e.StartDate, e.EndDate });

            model.HasOne(e => e.Unit)
                .WithMany(u => u.Leases)
                .HasForeignKey(e => e.UnitID)
                .OnDelete(DeleteBehavior.Restrict);

            model.HasOne(e => e.RentalApplication)
                .WithOne(a => a.Lease)
                .HasForeignKey<Lease>(e => e.RentalApplicationID)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
