using AutoMapper;
using Radency_TestAssignment.Application.DTOs;
using Radency_TestAssignment.Domain.Entities.Applications;

namespace Radency_TestAssignment.Application.AutoMapper.Profiles
{
    public class ResidenceHistoryProfiles : Profile
    {
        public ResidenceHistoryProfiles()
        {
            CreateMap<ResidenceHistory, ResidenceDTO>()
                .ForMember(d => d.ApplicationID, o => o.MapFrom(s => s.RentalApplicationID));

            CreateMap<ResidenceDTO, ResidenceHistory>()
                .ForMember(d => d.ID, o => o.Ignore())
                .ForMember(d => d.RentalApplicationID, o => o.Ignore());
        }
    }
}
