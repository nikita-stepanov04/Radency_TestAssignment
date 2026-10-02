using Radency_TestAssignment.Domain.Enums;

namespace Radency_TestAssignment.Web.Models.Applications
{
    public class ApplicationWizardViewModel
    {
        public int ID { get; set; }
        public ApplicationSection Step { get; set; }

        public bool IsEditable { get; set; }
        public string UnitTitle { get; set; } = string.Empty;
        public ApplicationStatus Status { get; set; }
        public List<bool> SavedSteps { get; set; } = new List<bool>();

        public ApplicantInfoSectionViewModel ApplicantInformation { get; set; } = new ApplicantInfoSectionViewModel();
        public ResidenceHistorySectionViewModel ResidenceHistory { get; set; } = new ResidenceHistorySectionViewModel();
        public SummaryViewModel Summary { get; set; } = new SummaryViewModel();

        public bool CanReview { get; set; }
        public string? ReviewComment { get; set; }
    }
}
