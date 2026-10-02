using Radency_TestAssignment.Domain.Applications;
using Radency_TestAssignment.Domain.Entities.Applications;

namespace Radency_TestAssignment.Infrastructure.IRepositories
{
    public interface IApplicationRepository : IRepositoryBase<RentalApplication>
    {
        Task<RentalApplication?> GetForEditAsync(int id, int userID);
        Task<RentalApplication?> GetForReviewAsync(int id);
        Task<ApplicationAccess?> GetAccessAsync(int id, int userID);
        Task<RentalApplication?> FindDraftAsync(int unitID, int userID);
        Task<bool> HasActiveLeaseAsync(int unitID, DateOnly today);
        Task<List<ApplicationListItem>> GetListAsync(ApplicationListFilter filter);
        Task<bool> TrySaveChangesAsync();
    }
}
