using Radency_TestAssignment.Domain.Entities.Applications;
using Radency_TestAssignment.Domain.Entities.Catalog;

namespace Radency_TestAssignment.Domain.Entities.Leasing
{
    public class Lease : EntityBase
    {
        public int UnitID { get; set; }
        public Unit Unit { get; set; } = null!;

        public int RentalApplicationID { get; set; }
        public RentalApplication RentalApplication { get; set; } = null!;

        public DateOnly StartDate { get; set; }

        public DateOnly EndDate { get; set; }
    }
}
