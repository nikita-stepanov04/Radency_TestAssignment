using Radency_TestAssignment.Application.DTOs;
using Radency_TestAssignment.Domain.Entities.Catalog;
using Radency_TestAssignment.Domain.Pagination;

namespace Radency_TestAssignment.Application.IBusinessServices
{
    public interface IUnitBS
    {
        Task<OpRes<int>> AddAsync(SaveUnitDTO unit);
        Task<OpRes<int>> UpdateAsync(SaveUnitDTO unit);
        Task DeleteAsync(int id);
        Task<Unit?> GetByIdAsync(int id);
        Task<PagedResult<UnitListItemDTO>> GetPagedAsync(int propID, PageRequest page);
        Task<List<UnitType>> GetUnitTypesAsync();
        Task SeedUnitsAsync();
    }
}
