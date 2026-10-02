using Radency_TestAssignment.Domain.Enums;

namespace Radency_TestAssignment.Application.DTOs
{
    public class ApplicationListItemDTO
    {
        public int ID { get; set; }
        public string PropertyName { get; set; } = string.Empty;
        public string UnitNumber { get; set; } = string.Empty;
        public string ApplicantNames { get; set; } = string.Empty;
        public ApplicationStatus Status { get; set; }
        public DateTime CreatedAtUtc { get; set; }
        public DateTime? SubmittedAtUtc { get; set; }
    }
}
