using Microsoft.AspNetCore.Identity;
using Radency_TestAssignment.Application;
using Radency_TestAssignment.Application.DTOs;
using Radency_TestAssignment.Application.IBusinessServices.Users;
using Radency_TestAssignment.Domain.Entities.Identity;
using Microsoft.EntityFrameworkCore;

namespace Radency_TestAssignment.Web.BusinessServices
{
    public class AccountBS(
    UserManager<User> _userManager,
    SignInManager<User> _signInManager,
    RoleManager<Role> _roleManager) : IAccountBS
    {
        public async Task<LogInStatus> LoginAsync(string email, string password)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null) return LogInStatus.InvalidEmail;

            var result = await _signInManager.PasswordSignInAsync(
                user, password, isPersistent: false, lockoutOnFailure: false);

            return result.Succeeded
                ? LogInStatus.Success
                : LogInStatus.InvalidPassword;
        }

        public async Task<OpRes<bool>> RegisterAsync(RegistrationDTO request)
        {
            if (request.Role != RoleNames.Applicant && request.Role != RoleNames.PropertyManager)
                return OpRes.Err<bool>("Invalid role");

            User user = new User
            {
                UserName = request.Email,
                Email = request.Email,
                FullName = request.FullName
            };

            IdentityResult created = await _userManager.CreateAsync(user, request.Password);

            if (!created.Succeeded)
                return OpRes.Err<bool>(created.Errors.Select(e => e.Description).First());

            IdentityResult roleAdded = await _userManager.AddToRoleAsync(user, request.Role);

            if (!roleAdded.Succeeded)
            {
                await _userManager.DeleteAsync(user);
                return OpRes.Err<bool>(roleAdded.Errors.Select(e => e.Description).First());
            }

            await _signInManager.SignInAsync(user, isPersistent: false);

            return OpRes.Success(true);
        }

        public Task LogoutAsync() => _signInManager.SignOutAsync();

        public async Task<List<string>> GetAllRolesAsync()
        {
            return await _roleManager.Roles
                .Select(r => r.Name!)
                .ToListAsync();
        }
    }
}
