using Microsoft.Extensions.DependencyInjection;
using Radency_TestAssignment.Application.IBusinessServices;

namespace Radency_TestAssignment.Application.BusinessServices
{
    public static class UnitTypeSeeder
    {
        public static async Task SeedUnitTypesAsync(IServiceProvider services)
        {
            var unitBS = services.CreateScope().ServiceProvider.GetRequiredService<IUnitBS>();
            await unitBS.SeedUnitsAsync();
        }
    }
}
