using Radency_TestAssignment.Application.IBusinessServices.Users;
using Radency_TestAssignment.Domain.DI;
using Radency_TestAssignment.Domain.Entities.Identity;
using Radency_TestAssignment.Web.BusinessServices;
using Radency_TestAssignment.Web.Identity;

namespace Radency_TestAssignment.Web.DI
{
    public class WebDIManager : IDependencyInjectionManager
    {
        public IServiceCollection SetupDI(IServiceCollection services, IConfiguration config)
        {
            services.AddControllersWithViews().AddJsonOptions(opts =>
            {
                opts.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
            });

            services.AddAutoMapper(cfg => { }, typeof(WebDIManager).Assembly);

            services.AddScoped<IAccountBS, AccountBS>();

            var identityManager = new IdentityManager();
            identityManager.SetupDI(services, config);

            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/User/Login";
                options.AccessDeniedPath = "/User/Login";
            });

            services.AddAuthorization(opts =>
            {
                opts.AddPolicy(Policies.AuthorizedManagers, p => p.RequireRole(RoleNames.PropertyManager));
                opts.AddPolicy(Policies.AuthorizedAny, p => p.RequireAuthenticatedUser());
            });

            services.AddRouting(options =>
            {
                options.LowercaseUrls = true;
                options.AppendTrailingSlash = false;
            });

            services.AddOptions<PaginationSettings>()
                .BindConfiguration("Pagination")
                .ValidateDataAnnotations()
                .ValidateOnStart();

            return services;
        }
    }
}
