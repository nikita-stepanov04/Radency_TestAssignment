using Radency_TestAssignment.Application.IBusinessServices.Users;
using Radency_TestAssignment.Domain.DI;
using Radency_TestAssignment.Web.BusinessServices;
using Radency_TestAssignment.Web.Identity;

namespace Radency_TestAssignment.Web.DI
{
    public class WebDIManager : IDependencyInjectionManager
    {
        public IServiceCollection SetupDI(IServiceCollection services, IConfiguration config)
        {
            services.AddAutoMapper(cfg => { }, typeof(WebDIManager).Assembly);

            services.AddScoped<IAccountBS, AccountBS>();

            var identityManager = new IdentityManager();
            identityManager.SetupDI(services, config);

            return services;
        }
    }
}
