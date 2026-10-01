using AutoMapper;
using Radency_TestAssignment.Application.DTOs;
using Radency_TestAssignment.Domain.Entities.Catalog;
using Radency_TestAssignment.Web.Models;

namespace Radency_TestAssignment.Web.AutoMapper.Profiles
{
    public class UnitProfiles : Profile
    {
        public UnitProfiles()
        {
            CreateMap<UnitFormViewModel, SaveUnitDTO>()
                .ForMember(d => d.PropertyID, o => o.MapFrom(s => s.PropertyID ?? 0))
                .ForMember(d => d.UnitTypeID, o => o.MapFrom(s => int.Parse(s.UnitTypeID!)))
                .ForMember(d => d.ID, o => o.MapFrom(s => s.ID ?? 0));

            CreateMap<Unit, UnitFormViewModel>()
                .ForMember(dest => dest.ID, opt => opt.MapFrom(src => src.ID))
                .ForMember(dest => dest.PropertyID, opt => opt.MapFrom(src => src.PropertyID))
                .ForMember(dest => dest.Number, opt => opt.MapFrom(src => src.UnitNumber))
                .ForMember(dest => dest.Bedrooms, opt => opt.MapFrom(src => src.Bedrooms))
                .ForMember(dest => dest.MonthlyRent, opt => opt.MapFrom(src => src.MonthlyRent))
                .ForMember(dest => dest.UnitTypeID, opt => opt.MapFrom(src => src.UnitTypeID.ToString()));
        }
    }
}
