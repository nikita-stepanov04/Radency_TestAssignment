using Microsoft.AspNetCore.Mvc;

namespace Radency_TestAssignment.Web.Controllers
{
    public class ApplicationController : Radency_TestAssignmentControllerBase
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
    }
}
