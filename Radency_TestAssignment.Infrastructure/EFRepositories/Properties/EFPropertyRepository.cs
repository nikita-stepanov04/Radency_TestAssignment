using Radency_TestAssignment.Domain.Entities.Catalog;
using Radency_TestAssignment.Infrastructure.IRepositories;

namespace Radency_TestAssignment.Infrastructure.EFRepositories.Properties
{
    public class EFPropertyRepository : EFRepositoryBase<Property>, IPropertyRepository
    {
        public EFPropertyRepository(EFDataContext context)
            : base(context) { }
    }
}
