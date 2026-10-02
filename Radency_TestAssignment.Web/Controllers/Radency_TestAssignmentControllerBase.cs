using Microsoft.AspNetCore.Mvc;
using Radency_TestAssignment.Web.Models.Shared;
using System.Security.Claims;

namespace Radency_TestAssignment.Web.Controllers
{
    public class Radency_TestAssignmentControllerBase : Controller
    {
        public void AlertDanger(string message) => AlertBase("danger", message);
        public void AlertSuccess(string message) => AlertBase("success", message);
        public void AlertWarning(string message) => AlertBase("warning", message);

        public void ConditionAlert(bool condition, string ifConditionMessage, string elseMessage)
        {
            if (condition)
                AlertSuccess(ifConditionMessage);
            else
                AlertDanger(elseMessage);
        }

        private void AlertBase(string alertType, string message)
        {
            TempData["AlertMessage"] = message;
            TempData["AlertType"] = alertType;
        }

        public const string ModalBindingPrefix = nameof(ModalFormViewModel<>.FormModel);

        public int UserID => int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
    }
}
