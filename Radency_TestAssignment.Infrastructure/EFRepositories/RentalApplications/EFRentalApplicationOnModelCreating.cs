using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Radency_TestAssignment.Domain.Entities.Applications;
using Radency_TestAssignment.Domain.Entities.Identity;
using Radency_TestAssignment.Infrastructure.EFRepository;

namespace Radency_TestAssignment.Infrastructure.EFRepositories.RentalApplications
{
    public class EFRentalApplicationOnModelCreating : EFOnModelCreatingBase<RentalApplication>
    {
        protected override void OnModelCreating(EntityTypeBuilder<RentalApplication> model)
        {
            model.HasIndex(e => e.Status);
            model.HasIndex(e => e.UnitID);

            model.Property(e => e.FullName).HasMaxLength(200);
            model.Property(e => e.Phone).HasMaxLength(50);
            model.Property(e => e.Email).HasMaxLength(256);
            model.Property(e => e.CurrentAddress).HasMaxLength(300);

            model.HasOne(e => e.Unit)
                .WithMany(u => u.Applications)
                .HasForeignKey(e => e.UnitID)
                .OnDelete(DeleteBehavior.Restrict);

            model.HasOne(e => e.CreatedBy)
                .WithMany()
                .HasForeignKey(e => e.CreatedByID)
                .OnDelete(DeleteBehavior.Restrict);

            model.HasOne(e => e.Reviewer)
                .WithMany()
                .HasForeignKey(e => e.ReviewerID)
                .OnDelete(DeleteBehavior.Restrict);

            model.HasMany(e => e.Applicants)
                .WithMany(u => u.Applications)
                .UsingEntity<Dictionary<string, object>>(
                    "ApplicationApplicant",
                    j => j.HasOne<User>()
                          .WithMany()
                          .HasForeignKey("UserID")
                          .OnDelete(DeleteBehavior.Cascade),
                    j => j.HasOne<RentalApplication>()
                          .WithMany()
                          .HasForeignKey("RentalApplicationID")
                          .OnDelete(DeleteBehavior.Cascade)
                );
        }
    }
}
