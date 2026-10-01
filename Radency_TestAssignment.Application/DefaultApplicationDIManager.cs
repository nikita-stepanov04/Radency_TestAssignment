using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Radency_TestAssignment.Application.BusinessServices;
using Radency_TestAssignment.Application.IBusinessServices.Properties;
using Radency_TestAssignment.Domain.DI;

namespace Radency_TestAssignment.Application
{
    public class DefaultApplicationDIManager : IDependencyInjectionManager
    {
        public IServiceCollection SetupDI(IServiceCollection services, IConfiguration config)
        {
            services.AddAutoMapper(cfg => { }, typeof(DefaultApplicationDIManager).Assembly);

            services.AddScoped<IPropertyBS, PropertyBS>();

            return services;
        }
    }
}
