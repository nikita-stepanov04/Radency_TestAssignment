using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Radency_TestAssignment.Domain.Entities.Catalog;
using Radency_TestAssignment.Infrastructure.EFRepository;

namespace Radency_TestAssignment.Infrastructure.EFRepositories.Properties
{
    public class EFPropertyOnModelCreating : EFOnModelCreatingBase<Property>
    {
        protected override void OnModelCreating(EntityTypeBuilder<Property> model)
        {
            model.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(200);

            model.Property(e => e.Address)
                .IsRequired()
                .HasMaxLength(300);

            model.Property(e => e.City)
                .IsRequired()
                .HasMaxLength(100);
        }
    }
}
