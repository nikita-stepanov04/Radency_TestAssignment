using AutoMapper;
using Radency_TestAssignment.Application.DTOs;
using Radency_TestAssignment.Application.IBusinessServices.Properties;
using Radency_TestAssignment.Domain.Entities.Catalog;
using Radency_TestAssignment.Domain.Pagination;
using Radency_TestAssignment.Infrastructure.IRepositories;

namespace Radency_TestAssignment.Application.BusinessServices
{
    public class PropertyBS(
        IMapper _mapper,
        IPropertyRepository _propRep) : IPropertyBS
    {
        public async Task<int> AddAsync(SavePropertyDTO dto)
        {
            var p = _mapper.Map<Property>(dto);

            await _propRep.AddAsync(p);
            await _propRep.SaveChangesAsync();

            return p.ID;
        }

        public async Task UpdateAsync(SavePropertyDTO dto)
        {
            var p = _mapper.Map<Property>(dto);

            _propRep.Update(p);
            await _propRep.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var p = await _propRep.GetByIDAsync(id);
            if (p != null)
            {
                _propRep.Delete(p);
            }
            await _propRep.SaveChangesAsync();
        }

        public Task<Property?> GetByIdAsync(int id)
        {
           return _propRep.GetByIDAsync(id);
        }

        public async Task<PagedResult<PropertyListItemDTO>> GetPagedAsync(PageRequest page)
        {
            var res = await _propRep.GetPropertiesPagedAsync(page);
            var properties = _mapper.Map<List<PropertyListItemDTO>>(res.Items);

            return new PagedResult<PropertyListItemDTO>(properties, res.TotalCount, res.Page, res.PageSize);
        }

        public async Task<List<PropertyLookupDTO>> GetLookupAsync()
            => (await _propRep.GetAllAsync())
                .OrderBy(p => p.Name)
                .Select(p => new PropertyLookupDTO()
                {
                    ID = p.ID,
                    Name = p.Name
                }).ToList();
    }
}
