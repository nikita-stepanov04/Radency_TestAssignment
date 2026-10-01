using AutoMapper;
using Radency_TestAssignment.Application.DTOs;
using Radency_TestAssignment.Application.IBusinessServices.Properties;
using Radency_TestAssignment.Domain.Entities.Catalog;
using Radency_TestAssignment.Infrastructure.IRepositories;

namespace Radency_TestAssignment.Application.BusinessServices
{
    public class PropertyBS(
        IMapper _mapper,
        IPropertyRepository _propRep) : IPropertyBS
    {
        public async Task<int> AddAsync(AddPropertyDTO dto)
        {
            var p = _mapper.Map<Property>(dto);

            await _propRep.AddAsync(p);
            await _propRep.SaveChangesAsync();

            return p.ID;
        }
    }
}
