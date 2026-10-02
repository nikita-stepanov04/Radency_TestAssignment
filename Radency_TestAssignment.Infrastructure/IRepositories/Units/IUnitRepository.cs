using Radency_TestAssignment.Domain.Entities.Catalog;
using Radency_TestAssignment.Domain.Pagination;

namespace Radency_TestAssignment.Infrastructure.IRepositories
{
    public interface IUnitRepository : IRepositoryBase<Unit>
    {
        Task<PagedResult<Unit>> GetUnitsPagedAsync(int propID, PageRequest page);
        Task<PagedResult<Unit>> GetAvailableUnitsPagedAsync(int userId, DateOnly today, PageRequest page);
        Task SeedTypesIfMissingAsync(IEnumerable<UnitType> types);
        Task<bool> IsUnitNumberTakenForPropertyAsync(string unitNumber, int propertyID, int? excludeUnitID = null);
        Task<List<UnitType>> GetUnitTypesAsync();
    }
}
