using AutoMapper;
using Radency_TestAssignment.Application.DTOs;
using Radency_TestAssignment.Web.Models;
using Radency_TestAssignment.Web.Models.Applications;

namespace Radency_TestAssignment.Application.AutoMapper.Profiles
{
    public class ApplicationProfile : Profile
    {
        public ApplicationProfile()
        {
            CreateMap<ApplicantInfoSectionViewModel, ApplicantInfoSectionDTO>().ReverseMap();

            CreateMap<ApplicationWizardViewModel, ApplicationWizardDTO>().ReverseMap();

            CreateMap<ApplicantInfoSectionViewModel, ApplicantInfoSectionDTO>().ReverseMap();
            CreateMap<ResidenceHistorySectionViewModel, ResidenceHistorySectionDTO>().ReverseMap();
            CreateMap<SummaryViewModel, SummaryDTO>().ReverseMap();

            CreateMap<ReviewFormViewModel, ReviewDTO>()
                .ForMember(d => d.ApplicationID, o => o.MapFrom(s => s.ID))
                .ForMember(d => d.Outcome, o => o.MapFrom(s => s.Outcome!.Value));
        }
    }
}
