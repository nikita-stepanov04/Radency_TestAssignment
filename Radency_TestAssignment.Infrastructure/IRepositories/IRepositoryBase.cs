using Microsoft.EntityFrameworkCore.Storage;
using Radency_TestAssignment.Domain.Entities;
using Radency_TestAssignment.Domain.Pagination;

namespace Radency_TestAssignment.Infrastructure.IRepositories
{
    public interface IRepositoryBase<TEntity> where TEntity : EntityBase
    {
        Task<TEntity?> GetByIDAsync(long id);
        Task AddAsync(TEntity entity);
        void Delete(TEntity entity);
        void Update(TEntity entity);
        Task<PagedResult<TProjection>> GetPagedAsync<TProjection>(
            IQueryable<TProjection> query, PageRequest page);
        Task SaveChangesAsync();
        Task<IDbContextTransaction> BeginTransactionAsync();
    }
}
