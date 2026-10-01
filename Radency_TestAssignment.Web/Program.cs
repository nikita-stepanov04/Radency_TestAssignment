using Radency_TestAssignment.Application;
using Radency_TestAssignment.Domain.DI;
using Radency_TestAssignment.Infrastructure.EFRepositories;
using Radency_TestAssignment.Web.Configuration;
using Radency_TestAssignment.Web.DI;
using Radency_TestAssignment.Web.Identity;

public partial class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var services = builder.Services;
        var config = builder.Configuration;

        builder.SetupLogger();        

        new List<IDependencyInjectionManager>
        {
            new EFInfrastructureDIManager(),
            new DefaultApplicationDIManager(),
            new WebDIManager()
        }.ForEach(di => di.SetupDI(services, config));

        var app = builder.Build();

        app.UseRouting();
        app.UseAuthorization();
        app.MapStaticAssets();
        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}"
        ).WithStaticAssets();

        await RoleSeeder.SeedRolesAsync(app.Services);

        app.Run();
    }
}