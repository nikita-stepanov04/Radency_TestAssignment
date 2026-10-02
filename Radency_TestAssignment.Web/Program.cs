using Microsoft.AspNetCore.Localization;
using Radency_TestAssignment.Application;
using Radency_TestAssignment.Application.BusinessServices;
using Radency_TestAssignment.Domain.DI;
using Radency_TestAssignment.Infrastructure.EFRepositories;
using Radency_TestAssignment.Web.Configuration;
using Radency_TestAssignment.Web.DI;
using Radency_TestAssignment.Web.Identity;
using System.Globalization;

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

        var culture = new CultureInfo("en-US");
        app.UseRequestLocalization(new RequestLocalizationOptions
        {
            DefaultRequestCulture = new RequestCulture(culture),
            SupportedCultures = [culture],
            SupportedUICultures = [culture]
        });

        app.UseRouting();
        app.UseAuthorization();
        app.MapStaticAssets();
        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}"
        ).WithStaticAssets();

        StartUpDb.ApplyMigrations(app.Services);

        await RoleSeeder.SeedRolesAsync(app.Services);
        await UnitTypeSeeder.SeedUnitTypesAsync(app.Services);

        app.Run();
    }
}