using Radency_TestAssignment.Domain.Entities.Applications;
using Radency_TestAssignment.Domain.Entities.Leasing;

namespace Radency_TestAssignment.Domain.Entities.Catalog
{
    public class Unit : EntityBase
    {
        public int PropertyID { get; set; }
        public Property Property { get; set; } = null!;

        public int UnitTypeID { get; set; }
        public UnitType UnitType { get; set; } = null!;

        public string UnitNumber { get; set; } = string.Empty;
        public int Bedrooms { get; set; }
        public decimal MonthlyRent { get; set; }

        public List<Lease> Leases { get; set; } = new List<Lease>();
        public List<RentalApplication> Applications { get; set; } = new List<RentalApplication>();
    }
}
