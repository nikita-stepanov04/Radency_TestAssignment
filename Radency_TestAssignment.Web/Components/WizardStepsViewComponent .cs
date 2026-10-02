using Microsoft.AspNetCore.Mvc;
using Radency_TestAssignment.Domain.Applications;
using Radency_TestAssignment.Web.Models.Applications;

namespace Radency_TestAssignment.Web.Components
{
    public class WizardStepsViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(ApplicationStep current, List<bool> saved)
        {
            var steps = new List<WizardStepItemViewModel>
            {
                Create(ApplicationStep.ApplicantInfo, "Applicant information", current, saved),
                Create(ApplicationStep.ResidenceHistory, "Residence history", current, saved),
                Create(ApplicationStep.Summary, "Summary", current, saved)
            };

            return View(steps);
        }

        private static WizardStepItemViewModel Create(ApplicationStep step, string title, ApplicationStep current, List<bool> saved)
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
