using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Radency_TestAssignment.Domain.Entities.Applications;
using Radency_TestAssignment.Infrastructure.EFRepository;

namespace Radency_TestAssignment.Infrastructure.EFRepositories.ManagerNotes
{
    public class EFManagerNoteOnModelCreating : EFOnModelCreatingBase<ManagerNote>
    {
        protected override void OnModelCreating(EntityTypeBuilder<ManagerNote> model)
        {
            model.Property(e => e.Text)
                .IsRequired()
                .HasMaxLength(2000);

            model.HasOne(e => e.RentalApplication)
                .WithMany(a => a.ManagerNotes)
                .HasForeignKey(e => e.RentalApplicationID)
                .OnDelete(DeleteBehavior.Cascade);

            model.HasOne(e => e.Author)
                .WithMany()
                .HasForeignKey(e => e.AuthorID)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
