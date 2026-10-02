using Radency_TestAssignment.Domain.Enums;

namespace Radency_TestAssignment.Web.Models.Applications
{
    public class WizardStepItemViewModel
    {
        public ApplicationSection Step { get; set; }
        public string Title { get; set; } = string.Empty;
        public bool IsCurrent { get; set; }
        public bool IsSaved { get; set; }
    }
}
