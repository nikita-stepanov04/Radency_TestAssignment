using Radency_TestAssignment.Domain.Entities.Identity;
using Radency_TestAssignment.Domain.Enums;

namespace Radency_TestAssignment.Domain.Entities.Applications
{
    public class ApplicationSectionState : EntityBase
    {
        public int RentalApplicationID { get; set; }
        public RentalApplication RentalApplication { get; set; } = null!;

        public ApplicationSection Section { get; set; }
        public bool IsSaved { get; set; }

        public Guid Version { get; set; } = Guid.NewGuid();

        public DateTime UpdatedAtUtc { get; set; } = DateTime.UtcNow;

        public int? UpdatedByID { get; set; }
        public User? UpdatedBy { get; set; }
    }
}
