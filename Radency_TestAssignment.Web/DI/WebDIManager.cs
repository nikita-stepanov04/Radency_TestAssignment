using Microsoft.AspNetCore.Identity;
using Radency_TestAssignment.Domain.DI;
using Radency_TestAssignment.Domain.Entities.Identity;

namespace Radency_TestAssignment.Web.DI
{
    public class WebDIManager : IDependencyInjectionManager
    {
        public IServiceCollection SetupDI(IServiceCollection services, IConfiguration config)
        {
            var identityBuilder = services.AddIdentity<User, Role>(options =>
                {
                    options.User.RequireUniqueEmail = true;
                    options.Password.RequiredLength = 8;
                })
                .AddDefaultTokenProviders();
            

            return services;
        }
    }
}
