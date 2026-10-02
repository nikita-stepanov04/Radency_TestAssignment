using Radency_TestAssignment.Application.DTOs;
using Radency_TestAssignment.Domain.Applications;
using Radency_TestAssignment.Domain.Pagination;

namespace Radency_TestAssignment.Application.IBusinessServices
{
    public interface IApplicationBS
    {
        Task<OpRes<int>> StartAsync(int unitID, int userID);
        Task<ApplicationAccess?> GetAccessAsync(int id, int userID);
        Task<ApplicationWizardDTO?> GetWizardAsync(int id, int userID, ApplicationStep step);
        Task FillContextAsync(ApplicationWizardDTO model, int userID);
        Task<OpRes<bool>> SaveApplicantInfoAsync(int id, int userID, ApplicantInfoSectionDTO dto);
        Task<OpRes<bool>> SaveResidenceHistoryAsync(int id, int userID, Guid version);
        Task<PagedResult<ApplicationListItemDTO>> GetListAsync(
            ApplicationListFilterDTO filter, int userID, bool isManager);
        Task<OpRes<bool>> SubmitAsync(int id, int userID);
    }
}
