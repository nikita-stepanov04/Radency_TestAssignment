using Radency_TestAssignment.Domain.Entities.Catalog;
using Radency_TestAssignment.Domain.Pagination;

namespace Radency_TestAssignment.Infrastructure.IRepositories
{
    public interface IPropertyRepository : IRepositoryBase<Property>
    {
        Task<PagedResult<Property>> GetPropertiesPagedAsync(PageRequest page);
    }
}
