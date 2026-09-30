using AutoMapper;
using Radency_TestAssignment.Application.DTOs;
using Radency_TestAssignment.Web.Models.Users;

namespace Radency_TestAssignment.Web.AutoMapper.Profiles
{
    public class AccountProfiles : Profile
    {
        public AccountProfiles()
        {
            CreateMap<RegistrationDTO, RegistrationViewModel>()
                .ReverseMap();
        }
    }
}
