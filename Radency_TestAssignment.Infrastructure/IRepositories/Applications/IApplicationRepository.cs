using Radency_TestAssignment.Domain.Applications;
using Radency_TestAssignment.Domain.Entities.Applications;
using Radency_TestAssignment.Domain.Pagination;

namespace Radency_TestAssignment.Infrastructure.IRepositories
{
    public interface IApplicationRepository : IRepositoryBase<RentalApplication>
    {
        Task<RentalApplication?> GetForEditAsync(int id, int userID);
        Task<ApplicationAccess?> GetAccessAsync(int id, int userID);
        Task<RentalApplication?> FindDraftAsync(int unitID, int userID);
        Task<bool> HasActiveLeaseAsync(int unitID, DateOnly today);
        Task<PagedResult<RentalApplication>> GetListAsync(ApplicationListQuery query, PageRequest page);
        Task<RentalApplication?> GetForViewAsync(int id, int userID, bool isManager);
        Task<ApplicationAccess?> GetViewAccessAsync(int id, int userID, bool isManager);
        Task<RentalApplication?> GetForReviewAsync(int id);
        Task<bool> TrySaveChangesAsync();
    }
}
