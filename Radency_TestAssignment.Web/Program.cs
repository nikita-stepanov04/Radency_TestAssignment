using Radency_TestAssignment.Domain.DI;
using Radency_TestAssignment.Infrastructure.EFRepositories;
using Radency_TestAssignment.Web.Configuration;
using Radency_TestAssignment.Web.DI;

public partial class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var services = builder.Services;
        var config = builder.Configuration;

        builder.SetupLogger();
        services.AddControllersWithViews();

        new List<IDependencyInjectionManager>
        {
            new EFInfrastructureDIManager(),
            new WebDIManager()
        }.ForEach(di => di.SetupDI(services, config));

        var app = builder.Build();

        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
        }

        app.UseRouting();
        app.UseAuthorization();
        app.MapStaticAssets();
        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}"
        ).WithStaticAssets();

        app.Run();
    }
}