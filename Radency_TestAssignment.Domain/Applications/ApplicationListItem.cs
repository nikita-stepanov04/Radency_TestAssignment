using Radency_TestAssignment.Domain.Enums;

namespace Radency_TestAssignment.Domain.Applications
{
    public class ApplicationListItem
    {
        public int ID { get; set; }
        public string PropertyName { get; set; } = string.Empty;
        public string UnitNumber { get; set; } = string.Empty;
        public ApplicationStatus Status { get; set; }
        public string ApplicantNames { get; set; } = string.Empty;
        public DateTime CreatedAtUtc { get; set; }
        public DateTime? SubmittedAtUtc { get; set; }
    }
}
