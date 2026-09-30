using Radency_TestAssignment.Domain.Entities.Catalog;
using Radency_TestAssignment.Domain.Entities.Identity;
using Radency_TestAssignment.Domain.Entities.Leasing;
using Radency_TestAssignment.Domain.Enums;

namespace Radency_TestAssignment.Domain.Entities.Applications
{
    public class RentalApplication : EntityBase
    {
        public int UnitID { get; set; }
        public Unit Unit { get; set; } = null!;

        public int CreatedByID { get; set; }
        public User CreatedBy { get; set; } = null!;

        public ApplicationStatus Status { get; set; } = ApplicationStatus.Draft;

        public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
        public DateTime? SubmittedAtUtc { get; set; }

        public string? FullName { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? CurrentAddress { get; set; }

        public int? ReviewerID { get; set; }
        public User? Reviewer { get; set; }
        public DateTime? ClaimedAtUtc { get; set; }

        public List<ResidenceHistory> Residences { get; set; } = new List<ResidenceHistory>();

        public List<ApplicationSectionState> SectionStates { get; set; } = new List<ApplicationSectionState>();

        public List<User> Applicants { get; set; } = new List<User>();

        public List<ManagerNote> ManagerNotes { get; set; } = new List<ManagerNote>();

        public Lease? Lease { get; set; }
    }
}
