using Microsoft.AspNetCore.Identity;
using Radency_TestAssignment.Domain.Entities.Identity;
using System.Reflection;

namespace Radency_TestAssignment.Web.Identity
{
    public static class RoleSeeder
    {
        public static async Task SeedRolesAsync(IServiceProvider services)
        {
            var roleManager = services.CreateScope().ServiceProvider.GetRequiredService<RoleManager<Role>>();

            var roles = typeof(RoleNames)
                .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
                .Where(fi => fi.IsLiteral && !fi.IsInitOnly)
                .Select(fi => (string)fi.GetRawConstantValue()!)
                .ToArray();

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                    await roleManager.CreateAsync(new Role(role));
            }
        }
    }
}
