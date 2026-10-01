using AutoMapper;
using Radency_TestAssignment.Application.DTOs;
using Radency_TestAssignment.Web.Models.Properties;

namespace Radency_TestAssignment.Web.AutoMapper.Profiles
{
    public class PropertyProfiles : Profile
    {
        public PropertyProfiles()
        {
            CreateMap<PropertyFormViewModel, AddPropertyDTO>().ReverseMap();
        }
    }
}
