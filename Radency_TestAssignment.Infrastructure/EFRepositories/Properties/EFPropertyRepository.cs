using Microsoft.EntityFrameworkCore;
using Radency_TestAssignment.Domain.Entities.Catalog;
using Radency_TestAssignment.Domain.Pagination;
using Radency_TestAssignment.Infrastructure.IRepositories;

namespace Radency_TestAssignment.Infrastructure.EFRepositories.Properties
{
    public class EFPropertyRepository : EFRepositoryBase<Property>, IPropertyRepository
    {
        public EFPropertyRepository(EFDataContext context)
            : base(context) { }

        public async Task<PagedResult<Property>> GetPropertiesPagedAsync(PageRequest page)
        {
            var query = DbSet.AsNoTracking()
                .OrderByDescending(p => p.ID);

            var result = await GetPagedAsync(query, page);

            return result;
        }
    }
}
