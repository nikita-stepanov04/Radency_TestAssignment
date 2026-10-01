using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Radency_TestAssignment.Domain.Entities;
using Radency_TestAssignment.Domain.Pagination;
using Radency_TestAssignment.Infrastructure.IRepositories;

namespace Radency_TestAssignment.Infrastructure.EFRepositories
{
    public class EFRepositoryBase<TEntity> : IRepositoryBase<TEntity>
        where TEntity : EntityBase
    {
        protected EFDataContext DbContext { get; }

        public DbSet<TEntity> DbSet => DbContext.Set<TEntity>();

        public EFRepositoryBase(EFDataContext context)
        {
            DbContext = context;
        }

        public virtual async Task AddAsync(TEntity entity)
        {
            await DbContext.Set<TEntity>().AddAsync(entity);
        }

        public virtual async Task<TEntity?> GetByIDAsync(int id)
        {
            return await DbContext.Set<TEntity>().FindAsync(id);
        }

        public virtual async Task SaveChangesAsync()
        {
            await DbContext.SaveChangesAsync();
        }

        public virtual void Delete(TEntity entity)
        {
            DbSet.Remove(entity);
        }

        public virtual void Update(TEntity entity)
        {
            DbSet.Update(entity);
        }

        public async Task<PagedResult<TProjection>> GetPagedAsync<TProjection>(
            IQueryable<TProjection> query, PageRequest page)
        {
            var total = await query.CountAsync();

            var totalPages = Math.Max(1, (int)Math.Ceiling(total / (double)page.PageSize));
            var current = Math.Clamp(page.Page, 1, totalPages);

            var items = await query
                .Skip((current - 1) * page.PageSize)
                .Take(page.PageSize)
                .ToListAsync();

            return new PagedResult<TProjection>(items, total, current, page.PageSize);
        }

        public Task<IDbContextTransaction> BeginTransactionAsync()
        {
            return DbContext.Database.BeginTransactionAsync();
        }

        public Task<List<TEntity>> GetAllAsync()
        {
            return DbSet.ToListAsync();
        }
    }
}
