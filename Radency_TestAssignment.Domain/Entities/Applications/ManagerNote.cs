using Radency_TestAssignment.Domain.Entities.Identity;

namespace Radency_TestAssignment.Domain.Entities.Applications
{
    public class ManagerNote : EntityBase
    {
        public int RentalApplicationID { get; set; }
        public RentalApplication RentalApplication { get; set; } = null!;

        public int AuthorID { get; set; }
        public User Author { get; set; } = null!;

        public string Text { get; set; } = string.Empty;

        public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAtUtc { get; set; }
    }
}
