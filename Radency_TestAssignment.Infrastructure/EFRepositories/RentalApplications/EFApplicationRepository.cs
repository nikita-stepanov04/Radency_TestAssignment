using Microsoft.EntityFrameworkCore;
using Radency_TestAssignment.Domain.Applications;
using Radency_TestAssignment.Domain.Entities.Applications;
using Radency_TestAssignment.Domain.Entities.Leasing;
using Radency_TestAssignment.Domain.Enums;
using Radency_TestAssignment.Infrastructure.IRepositories;

namespace Radency_TestAssignment.Infrastructure.EFRepositories
{
    public class EFApplicationRepository : EFRepositoryBase<RentalApplication>, IApplicationRepository
    {
        public EFApplicationRepository(EFDataContext context)
            : base(context) { }

        public async Task<RentalApplication?> GetForEditAsync(int id, int userID)
        {
            return await DbSet
                .Include(a => a.Unit).ThenInclude(u => u.Property)
                .Include(a => a.SectionStates)
                .Include(a => a.Residences.OrderBy(r => r.MoveInDate))
                .AsSplitQuery()
                .SingleOrDefaultAsync(a => a.ID == id && a.Applicants.Any(u => u.Id == userID));
        }

        public async Task<RentalApplication?> GetForReviewAsync(int id)
        {
            return await DbSet
                .Include(a => a.Unit).ThenInclude(u => u.Property)
                .Include(a => a.SectionStates)
                .Include(a => a.Residences.OrderBy(r => r.MoveInDate))
                .Include(a => a.Applicants)
                .Include(a => a.Reviewer)
                .AsSplitQuery()
                .SingleOrDefaultAsync(a => a.ID == id);
        }

        public async Task<ApplicationAccess?> GetAccessAsync(int id, int userID)
        {
            return await DbSet
                .AsNoTracking()
                .Where(a => a.ID == id && a.Applicants.Any(u => u.Id == userID))
                .Select(a => new ApplicationAccess { ID = a.ID, Status = a.Status })
                .SingleOrDefaultAsync();
        }

        public async Task<RentalApplication?> FindDraftAsync(int unitID, int userID)
        {
            return await DbSet
                .FirstOrDefaultAsync(a => a.UnitID == unitID
                    && a.Status == ApplicationStatus.Draft
                    && a.Applicants.Any(u => u.Id == userID));
        }

        public async Task<bool> HasActiveLeaseAsync(int unitID, DateOnly today)
        {
            return await DbContext.Set<Lease>()
                .AnyAsync(l => l.UnitID == unitID && l.StartDate <= today && today <= l.EndDate);
        }

        public async Task<List<ApplicationListItem>> GetListAsync(ApplicationListFilter filter)
        {
            var query = DbSet.AsNoTracking();

            if (filter.ApplicantID.HasValue)
                query = query.Where(a => a.Applicants.Any(u => u.Id == filter.ApplicantID.Value));

            if (filter.Status.HasValue)
                query = query.Where(a => a.Status == filter.Status.Value);

            if (filter.PropertyID.HasValue)
                query = query.Where(a => a.Unit.PropertyID == filter.PropertyID.Value);

            return await query
                .OrderByDescending(a => a.SubmittedAtUtc ?? a.CreatedAtUtc)
                .Select(a => new ApplicationListItem
                {
                    ID = a.ID,
                    PropertyName = a.Unit.Property.Name,
                    UnitNumber = a.Unit.UnitNumber,
                    Status = a.Status,
                    ApplicantNames = string.Join(", ", a.Applicants.Select(u => u.FullName)),
                    CreatedAtUtc = a.CreatedAtUtc,
                    SubmittedAtUtc = a.SubmittedAtUtc
                })
                .ToListAsync();
        }
        public async Task<bool> TrySaveChangesAsync()
        {
            try
            {
                await SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                return false;
            }
        }
    }
}
