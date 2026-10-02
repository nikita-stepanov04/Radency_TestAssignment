using Microsoft.EntityFrameworkCore;
using Radency_TestAssignment.Domain.Applications;
using Radency_TestAssignment.Domain.Entities.Applications;
using Radency_TestAssignment.Domain.Entities.Leasing;
using Radency_TestAssignment.Domain.Enums;
using Radency_TestAssignment.Domain.Pagination;
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

        public async Task<PagedResult<RentalApplication>> GetListAsync(ApplicationListQuery query, PageRequest page)
        {
            var applications = DbSet.AsNoTracking();

            if (query.ApplicantID.HasValue)
                applications = applications.Where(a => a.Applicants.Any(u => u.Id == query.ApplicantID.Value));

            if (query.ExcludeDrafts)
                applications = applications.Where(a => a.Status != ApplicationStatus.Draft);

            if (query.Status.HasValue)
                applications = applications.Where(a => a.Status == query.Status.Value);

            if (query.PropertyID.HasValue)
                applications = applications.Where(a => a.Unit.PropertyID == query.PropertyID.Value);

            var ordered = applications
                .Include(a => a.Unit).ThenInclude(u => u.Property)
                .Include(a => a.Applicants)
                .OrderByDescending(a => a.SubmittedAtUtc ?? a.CreatedAtUtc)
                .ThenByDescending(a => a.ID);

            return await GetPagedAsync(ordered, page);
        }

        private IQueryable<RentalApplication> ViewableBy(int userID, bool isManager)
        {
            var applications = DbSet.AsNoTracking();

            return isManager
                ? applications.Where(a => a.Status != ApplicationStatus.Draft)
                : applications.Where(a => a.Applicants.Any(u => u.Id == userID));
        }

        public async Task<RentalApplication?> GetForViewAsync(int id, int userID, bool isManager)
        {
            return await ViewableBy(userID, isManager)
                .Include(a => a.Unit)
                    .ThenInclude(u => u.Property)
                .Include(a => a.SectionStates)
                .Include(a => a.ManagerNotes)
                .AsSplitQuery()
                .SingleOrDefaultAsync(a => a.ID == id);
        }

        public async Task<ApplicationAccess?> GetViewAccessAsync(int id, int userID, bool isManager)
        {
            return await ViewableBy(userID, isManager)
                .Where(a => a.ID == id)
                .Select(a => new ApplicationAccess { ID = a.ID, Status = a.Status })
                .SingleOrDefaultAsync();
        }

        public async Task<RentalApplication?> GetForReviewAsync(int id)
        {
            return await DbSet.SingleOrDefaultAsync(a => a.ID == id);
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
