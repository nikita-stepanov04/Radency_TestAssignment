using Radency_TestAssignment.Domain.Applications;
using Radency_TestAssignment.Domain.Enums;

namespace Radency_TestAssignment.Application.DTOs
{
    public class ApplicationWizardDTO
    {
        public int ID { get; set; }
        public ApplicationStep Step { get; set; }

        public bool IsEditable { get; set; }
        public string UnitTitle { get; set; } = string.Empty;
        public ApplicationStatus Status { get; set; }
        public List<bool> SavedSteps { get; set; } = new List<bool>();

        public ApplicantInfoSectionDTO ApplicantInformation { get; set; } = new ApplicantInfoSectionDTO();
        public ResidenceHistorySectionDTO ResidenceHistory { get; set; } = new ResidenceHistorySectionDTO();
        public SummaryDTO Summary { get; set; } = new SummaryDTO();
    }


}
