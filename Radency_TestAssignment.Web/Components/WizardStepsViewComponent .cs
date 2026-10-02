using Microsoft.AspNetCore.Mvc;
using Radency_TestAssignment.Domain.Enums;
using Radency_TestAssignment.Web.Models.Applications;

namespace Radency_TestAssignment.Web.Components
{
    public class WizardStepsViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(ApplicationSection current, List<bool> saved)
        {
            var steps = new List<WizardStepItemViewModel>
            {
                Create(ApplicationSection.ApplicantInformation, "Applicant information", current, saved),
                Create(ApplicationSection.ResidenceHistory, "Residence history", current, saved),
                Create(ApplicationSection.Summary, "Summary", current, saved)
            };

            return View(steps);
        }

        private static WizardStepItemViewModel Create(ApplicationSection step, string title, ApplicationSection current, List<bool> saved)
        {
            var index = (int)step;

            return new WizardStepItemViewModel
            {
                Step = step,
                Title = title,
                IsCurrent = step == current,
                IsSaved = index < saved.Count && saved[index]
            };
        }
    }
}
