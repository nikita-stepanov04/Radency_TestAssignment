using AutoMapper;
using Radency_TestAssignment.Application.DTOs;
using Radency_TestAssignment.Domain.Entities.Catalog;

namespace Radency_TestAssignment.Application.AutoMapper.Profiles
{
    public class UnitProfiles : Profile
    {
        public UnitProfiles()
        {
            CreateMap<Unit, UnitListItemDTO>()
                .ForMember(dest => dest.ID, opt => opt.MapFrom(src => src.ID))
                .ForMember(dest => dest.PropertyID, opt => opt.MapFrom(src => src.PropertyID))
                .ForMember(dest => dest.UnitNumber, opt => opt.MapFrom(src => src.UnitNumber))
                .ForMember(dest => dest.UnitType, opt => opt.MapFrom(src => src.UnitType.Name))
                .ForMember(dest => dest.Bedrooms, opt => opt.MapFrom(src => src.Bedrooms))
                .ForMember(dest => dest.MonthlyRent, opt => opt.MapFrom(src => src.MonthlyRent))
                .ForMember(dest => dest.IsAvailable, opt => opt.MapFrom(src => !src.Leases.Any()));

            CreateMap<SaveUnitDTO, Unit>()
                .ForMember(dest => dest.ID, opt => opt.MapFrom(src => src.ID))
                .ForMember(dest => dest.PropertyID, opt => opt.MapFrom(src => src.PropertyID))
                .ForMember(dest => dest.UnitNumber, opt => opt.MapFrom(src => src.Number))
                .ForMember(dest => dest.Bedrooms, opt => opt.MapFrom(src => src.Bedrooms))
                .ForMember(dest => dest.MonthlyRent, opt => opt.MapFrom(src => src.MonthlyRent))
                .ForMember(dest => dest.UnitTypeID, opt => opt.MapFrom(src => src.UnitTypeID));
        }
    }
}

