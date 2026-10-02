using Microsoft.EntityFrameworkCore;
using Radency_TestAssignment.Domain.Entities.Catalog;
using Radency_TestAssignment.Domain.Enums;
using Radency_TestAssignment.Domain.Pagination;
using Radency_TestAssignment.Infrastructure.IRepositories;

namespace Radency_TestAssignment.Infrastructure.EFRepositories
{
    public class EFUnitRepository : EFRepositoryBase<Unit>, IUnitRepository
    {
        public EFUnitRepository(EFDataContext context)
            : base(context) { }

        public async Task<PagedResult<Unit>> GetUnitsPagedAsync(int propID, PageRequest page)
        {
            var query = DbSet.AsNoTracking()
                .Include(u => u.UnitType)
                .Include(u => u.Leases)
                .Where(u => u.PropertyID == propID)
                .OrderBy(u => u.UnitNumber);

            var result = await GetPagedAsync(query, page);

            return result;
        }

        private static readonly ApplicationStatus[] OpenStatuses =
        {
            ApplicationStatus.Draft,
            ApplicationStatus.Returned,
            ApplicationStatus.Submitted
        };

        public async Task<PagedResult<Unit>> GetAvailableUnitsPagedAsync(int userId, DateOnly today, PageRequest page)
        {
            var query = DbSet.AsNoTracking()
                .Include(u => u.Property)
                .Include(u => u.UnitType)
                .Include(u => u.Applications
                    .Where(a => OpenStatuses.Contains(a.Status)
                             && a.Applicants.Any(x => x.Id == userId)))
                .Where(u => !u.Leases.Any(l => l.StartDate <= today && today <= l.EndDate))
                .OrderBy(u => u.Property.Name)
                .ThenBy(u => u.UnitNumber);

            return await GetPagedAsync(query, page);
        }

        public Task<List<UnitType>> GetUnitTypesAsync()
        {
            return DbContext.Set<UnitType>()
                .Where(u => u.IsActive)
                .ToListAsync();
        }

        public Task<bool> IsUnitNumberTakenForPropertyAsync(string unitNumber, int propertyID, int? excludeUnitID = null)
        {
            if (string.IsNullOrWhiteSpace(unitNumber))
                return Task.FromResult(true);

            var query = DbSet.AsQueryable()
                .Where(u => u.PropertyID == propertyID && u.UnitNumber == unitNumber);

            if (excludeUnitID.HasValue)
                query = query.Where(u => u.ID != excludeUnitID.Value);

            return query.AnyAsync();
        }


        public async Task SeedTypesIfMissingAsync(IEnumerable<UnitType> types)
        {
            foreach (var type in types)
            {
                if (!await DbContext.Set<UnitType>().AnyAsync(t => t.Name == type.Name))
                    DbContext.Set<UnitType>().Add(new UnitType { Name = type.Name, IsActive = type.IsActive});
            }
        }
    }
}
