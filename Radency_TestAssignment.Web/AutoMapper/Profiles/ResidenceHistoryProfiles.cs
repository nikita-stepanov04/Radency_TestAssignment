using AutoMapper;
using Radency_TestAssignment.Application.DTOs;
using Radency_TestAssignment.Web.Models;
using Radency_TestAssignment.Web.Models.Applications;

namespace Radency_TestAssignment.Web.AutoMapper.Profiles
{
    public class ResidenceHistoryProfiles : Profile
    {
        public ResidenceHistoryProfiles()
        {
            CreateMap<ResidenceDTO, ResidenceFormViewModel>().ReverseMap();
            CreateMap<ResidenceDTO, ResidenceListItemViewModel>();
        }
    }
}
