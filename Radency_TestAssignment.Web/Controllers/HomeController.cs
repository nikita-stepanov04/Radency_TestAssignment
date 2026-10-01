using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Radency_TestAssignment.Domain.Entities.Identity;
using Radency_TestAssignment.Web.Identity;

namespace Radency_TestAssignment.Web.Controllers
{
    [Authorize(Policy = Policies.AuthorizedAny)]
    public class HomeController : Radency_TestAssignmentControllerBase
    {
        [HttpGet]
        public IActionResult Index()
        {
            return User.IsInRole(RoleNames.PropertyManager)
                ? RedirectToAction("Index", "Property")
                : RedirectToAction("Index", "Home");
        }
    }
}
