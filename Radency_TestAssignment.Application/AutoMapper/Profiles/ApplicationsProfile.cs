using AutoMapper;
using Radency_TestAssignment.Application.DTOs;
using Radency_TestAssignment.Domain.Entities.Applications;

namespace Radency_TestAssignment.Application.AutoMapper.Profiles
{
    public class ApplicationsProfile : Profile
    {
        public ApplicationsProfile()
        {
            CreateMap<RentalApplication, ApplicationListItemDTO>()
                .ForMember(d => d.PropertyName, o => o.MapFrom(s => s.Unit.Property.Name))
                .ForMember(d => d.UnitNumber, o => o.MapFrom(s => s.Unit.UnitNumber))
                .ForMember(d => d.ApplicantNames, o => o.MapFrom(s => string.Join(", ", s.Applicants.Select(u => u.FullName))));
        }
    }
}
