using Radency_TestAssignment.Application.DTOs;

namespace Radency_TestAssignment.Application.IBusinessServices.Users
{
    public interface IAccountBS
    {
        Task<LogInStatus> LoginAsync(string email, string password);
        Task<OpRes<bool>> RegisterAsync(RegistrationDTO request);
        Task<List<string>> GetAllRolesAsync();
        Task LogoutAsync();
    }
}
