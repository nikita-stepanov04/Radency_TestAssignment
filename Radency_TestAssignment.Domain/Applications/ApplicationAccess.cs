using Radency_TestAssignment.Domain.Enums;

namespace Radency_TestAssignment.Domain.Applications
{
    public class ApplicationAccess
    {
        public int ID { get; set; }
        public ApplicationStatus Status { get; set; }
        public bool IsEditable => ApplicationRules.CanEdit(Status);
    }
}
