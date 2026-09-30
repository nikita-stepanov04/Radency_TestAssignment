using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Radency_TestAssignment.Domain.DI
{
    public interface IDependencyInjectionManager
    {
        IServiceCollection SetupDI(IServiceCollection services, IConfiguration config);
    }
}
