using Microsoft.EntityFrameworkCore;
using Radency_TestAssignment.Domain.Entities.Applications;
using Radency_TestAssignment.Infrastructure.IRepositories;

namespace Radency_TestAssignment.Infrastructure.EFRepositories
{
    public class EFResidenceHistoryRepository : EFRepositoryBase<ResidenceHistory>, IResidenceHistoryRepository
    {
        public EFResidenceHistoryRepository(EFDataContext context)
            : base(context) { }

        public Task<List<ResidenceHistory>> GetByApplicationAsync(int applicationID)
        {
            return DbSet
                .AsNoTracking()
                .Where(r => r.RentalApplicationID == applicationID)
                .OrderByDescending(r => r.MoveInDate)
                .ThenBy(r => r.ID)
                .ToListAsync();
        }
        public async Task<int?> GetApplicationIDAsync(int id)
        {
            return await DbSet.AsNoTracking()
                .Where(r => r.ID == id)
                .Select(r => (int?)r.RentalApplicationID)
                .SingleOrDefaultAsync();
        }
    }
}
