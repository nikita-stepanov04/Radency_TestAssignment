using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Radency_TestAssignment.Domain.Entities.Applications;
using Radency_TestAssignment.Infrastructure.EFRepository;

namespace Radency_TestAssignment.Infrastructure.EFRepositories.ResidenceHistories
{
    public class EFResidenceHistoryOnModelCreating : EFOnModelCreatingBase<ResidenceHistory>
    {
        protected override void OnModelCreating(EntityTypeBuilder<ResidenceHistory> model)
        {
            model.Property(e => e.Address).HasMaxLength(300);
            model.Property(e => e.LandlordName).HasMaxLength(200);
            model.Property(e => e.LandlordPhone).HasMaxLength(50);

            model.HasOne(e => e.RentalApplication)
                .WithMany(a => a.Residences)
                .HasForeignKey(e => e.RentalApplicationID)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
