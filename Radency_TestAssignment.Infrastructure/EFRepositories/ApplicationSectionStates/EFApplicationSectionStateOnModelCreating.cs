using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Radency_TestAssignment.Domain.Entities.Applications;
using Radency_TestAssignment.Infrastructure.EFRepository;

namespace Radency_TestAssignment.Infrastructure.EFRepositories.ApplicationSectionStates
{
    public class EFApplicationSectionStateOnModelCreating : EFOnModelCreatingBase<ApplicationSectionState>
    {
        protected override void OnModelCreating(EntityTypeBuilder<ApplicationSectionState> model)
        {
            model.Property(e => e.Version)
                .IsConcurrencyToken();

            model.HasIndex(e => new { e.RentalApplicationID, e.Section })
                .IsUnique();

            model.HasOne(e => e.RentalApplication)
                .WithMany(a => a.SectionStates)
                .HasForeignKey(e => e.RentalApplicationID)
                .OnDelete(DeleteBehavior.Cascade);

            model.HasOne(e => e.UpdatedBy)
                .WithMany()
                .HasForeignKey(e => e.UpdatedByID)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
