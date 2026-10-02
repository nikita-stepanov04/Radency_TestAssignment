using Radency_TestAssignment.Domain.Enums;

namespace Radency_TestAssignment.Domain.Applications
{
    public class ApplicationListFilter
    {
        public ApplicationStatus? Status { get; set; }
        public int? PropertyID { get; set; }
        public int? ApplicantID { get; set; }
    }
}
