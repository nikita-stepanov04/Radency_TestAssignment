using Radency_TestAssignment.Application.DTOs;

namespace Radency_TestAssignment.Application.IBusinessServices
{
    public interface IResidenceHistoryBS
    {
        Task<OpRes<List<ResidenceDTO>>> GetListAsync(int applicationID, int userID);
        Task<OpRes<ResidenceDTO>> GetForEditAsync(int id, int userID);
        Task<OpRes<bool>> CreateAsync(int userID, ResidenceDTO dto);
        Task<OpRes<bool>> UpdateAsync(int userID, ResidenceDTO dto);
        Task<OpRes<bool>> DeleteAsync(int userID, int id);
    }
}
