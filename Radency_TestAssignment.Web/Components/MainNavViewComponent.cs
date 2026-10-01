using Microsoft.AspNetCore.Mvc;
using Radency_TestAssignment.Domain.Entities.Identity;
using Radency_TestAssignment.Web.Models.Shared;

namespace Radency_TestAssignment.Web.Components
{
    public class MainNavViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            var model = new MainNavViewModel
            {
                IsAuthenticated = User.Identity?.IsAuthenticated == true,
                Email = User.Identity?.Name,
                IsManager = User.IsInRole(RoleNames.PropertyManager),
                IsApplicant = User.IsInRole(RoleNames.Applicant)
            };
            return View(model);
        }
    }
}
