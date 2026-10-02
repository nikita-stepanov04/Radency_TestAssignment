using Radency_TestAssignment.Domain.Entities.Applications;

namespace Radency_TestAssignment.Infrastructure.IRepositories
{
    public interface IResidenceHistoryRepository : IRepositoryBase<ResidenceHistory>
    {
        Task<List<ResidenceHistory>> GetByApplicationAsync(int applicationID);
        Task<int?> GetApplicationIDAsync(int id);
    }
}
