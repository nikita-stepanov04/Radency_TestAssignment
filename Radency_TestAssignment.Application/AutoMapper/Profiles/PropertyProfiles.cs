using AutoMapper;
using Radency_TestAssignment.Application.DTOs;
using Radency_TestAssignment.Domain.Entities.Catalog;

namespace Radency_TestAssignment.Application.AutoMapper.Profiles
{
    public class PropertyProfiles : Profile
    {
        public PropertyProfiles()
        {
            CreateMap<Property, AddPropertyDTO>().ReverseMap();
        }
    }
}
