using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Radency_TestAssignment.Domain.Entities.Catalog;
using Radency_TestAssignment.Infrastructure.EFRepository;

namespace Radency_TestAssignment.Infrastructure.EFRepositories.Units
{
    public class EFUnitOnModelCreating : EFOnModelCreatingBase<Unit>
    {
        protected override void OnModelCreating(EntityTypeBuilder<Unit> model)
        {
            model.Property(e => e.UnitNumber)
                .IsRequired()
                .HasMaxLength(50);

            model.Property(e => e.MonthlyRent)
                .HasColumnType("numeric(12,2)");

            model.HasIndex(e => new { e.PropertyID, e.UnitNumber })
                .IsUnique();

            model.HasOne(e => e.Property)
                .WithMany(p => p.Units)
                .HasForeignKey(e => e.PropertyID)
                .OnDelete(DeleteBehavior.Cascade);

            model.HasOne(e => e.UnitType)
                .WithMany(t => t.Units)
                .HasForeignKey(e => e.UnitTypeID)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
