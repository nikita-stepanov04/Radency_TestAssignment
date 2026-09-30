using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Radency_TestAssignment.Domain.DI;
using Radency_TestAssignment.Domain.Entities.Identity;

namespace Radency_TestAssignment.Infrastructure.EFRepositories
{
    public class EFInfrastructureDIManager : IDependencyInjectionManager
    {
        public IServiceCollection SetupDI(IServiceCollection services, IConfiguration config)
        {
            string? dbConnection = config.GetConnectionString("DbConnection");

            if (dbConnection == null) throw new ArgumentNullException("DbConnection is not defined");

            services.AddDbContext<EFDataContext>(opts =>
            {
                opts.UseSqlServer(dbConnection, dbOpts => dbOpts.MigrationsAssembly("Radency_TestAssignment.Infrastructure"));
#if DEBUG
                opts.EnableSensitiveDataLogging();
#endif
            });

            new IdentityBuilder(typeof(User), typeof(Role), services)
                .AddEntityFrameworkStores<EFDataContext>();

            return services;
        }
    }

    public static class StartUpDb
    {
        public static void ApplyMigrations(this IServiceProvider services)
        {
            using (var scope = services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<EFDataContext>();
                db.Database.Migrate();
            }
        }
    }
}
