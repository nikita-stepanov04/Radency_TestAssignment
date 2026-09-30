using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Radency_TestAssignment.Domain.Entities.Catalog;
using Radency_TestAssignment.Infrastructure.EFRepository;

namespace Radency_TestAssignment.Infrastructure.EFRepositories.UnitTypes
{
    public class EFUnitTypeOnModelCreating : EFOnModelCreatingBase<UnitType>
    {
        protected override void OnModelCreating(EntityTypeBuilder<UnitType> model)
        {
            model.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(100);

            model.HasIndex(e => e.Name)
                .IsUnique();
        }
    }
}
