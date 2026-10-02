using Radency_TestAssignment.Application.DTOs;
using Radency_TestAssignment.Domain.Entities.Catalog;
using Radency_TestAssignment.Domain.Pagination;

namespace Radency_TestAssignment.Application.IBusinessServices.Properties
{
    public interface IPropertyBS
    {
        Task<int> AddAsync(SavePropertyDTO property);
        Task UpdateAsync(SavePropertyDTO property);
        Task DeleteAsync(int id);
        Task<Property?> GetByIdAsync(int id);
        Task<PagedResult<PropertyListItemDTO>> GetPagedAsync(PageRequest page);
        Task<List<PropertyLookupDTO>> GetLookupAsync();
    }
}
