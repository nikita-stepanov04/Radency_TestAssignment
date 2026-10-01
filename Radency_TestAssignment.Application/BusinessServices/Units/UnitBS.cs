using AutoMapper;
using Radency_TestAssignment.Application.DTOs;
using Radency_TestAssignment.Application.IBusinessServices;
using Radency_TestAssignment.Domain.Entities.Catalog;
using Radency_TestAssignment.Domain.Pagination;
using Radency_TestAssignment.Infrastructure.IRepositories;

namespace Radency_TestAssignment.Application.BusinessServices
{
    public class UnitBS(
        IMapper _mapper,
        IUnitRepository _unitRep) : IUnitBS
    {
        public async Task<OpRes<int>> AddAsync(SaveUnitDTO dto)
        {
            var p = _mapper.Map<Unit>(dto);

            var res = await _unitRep.IsUnitNumberTakenForPropertyAsync(dto.Number, dto.PropertyID);
            if (res) return OpRes.Err<int>($"Unit {dto.Number} is already added to this property");

            await _unitRep.AddAsync(p);
            await _unitRep.SaveChangesAsync();

            return OpRes.Success(p.ID);
        }

        public async Task<OpRes<int>> UpdateAsync(SaveUnitDTO dto)
        {
            var p = _mapper.Map<Unit>(dto);

            var res = await _unitRep.IsUnitNumberTakenForPropertyAsync(dto.Number, dto.PropertyID, dto.ID);
            if (res) return OpRes.Err<int>($"Unit {dto.Number} is already added to this property");

            _unitRep.Update(p);
            await _unitRep.SaveChangesAsync();
            return OpRes.Success(p.ID);
        }

        public async Task DeleteAsync(int id)
        {
            var p = await _unitRep.GetByIDAsync(id);
            if (p != null)
            {
                _unitRep.Delete(p);
            }
            await _unitRep.SaveChangesAsync();
        }

        public Task<Unit?> GetByIdAsync(int id)
        {
            return _unitRep.GetByIDAsync(id);
        }

        public async Task<PagedResult<UnitListItemDTO>> GetPagedAsync(int propID, PageRequest page)
        {
            var res = await _unitRep.GetUnitsPagedAsync(propID, page);
            var units = _mapper.Map<List<UnitListItemDTO>>(res.Items);

            return new PagedResult<UnitListItemDTO>(units, res.TotalCount, res.Page, res.PageSize);
        }

        public Task<List<UnitType>> GetUnitTypesAsync()
        {
            return _unitRep.GetUnitTypesAsync();
        }

        public async Task SeedUnitsAsync()
        {
            var types = new UnitType[]
            {
                new UnitType("Studio", true), 
                new UnitType("One Bedroom", true),
                new UnitType("Two Bedroom", true),
                new UnitType("Penthouse", true), 
                new UnitType("Loft", false)
            };

            await _unitRep.SeedTypesIfMissingAsync(types);
            await _unitRep.SaveChangesAsync();
        }
    }
}

