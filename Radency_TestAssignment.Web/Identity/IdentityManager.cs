using Microsoft.AspNetCore.Identity;
using Radency_TestAssignment.Domain.DI;
using Radency_TestAssignment.Domain.Entities.Identity;

namespace Radency_TestAssignment.Web.Identity
{
    public class IdentityManager : IDependencyInjectionManager
    {
        public IServiceCollection SetupDI(IServiceCollection services, IConfiguration config)
        {
            var identityBuilder = services.AddIdentity<User, Role>(options =>
            {
                options.User.RequireUniqueEmail = true;
                options.Password.RequiredLength = 4;
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredUniqueChars = 0;
            }).AddDefaultTokenProviders();
            return services;
        }        
    }
}
