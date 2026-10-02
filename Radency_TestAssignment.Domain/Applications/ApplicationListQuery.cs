using Radency_TestAssignment.Domain.Enums;

namespace Radency_TestAssignment.Domain.Applications
{
    public class ApplicationListQuery
    {
        public int? ApplicantID { get; set; }
        public bool ExcludeDrafts { get; set; }
        public ApplicationStatus? Status { get; set; }
        public int? PropertyID { get; set; }
    }
}
