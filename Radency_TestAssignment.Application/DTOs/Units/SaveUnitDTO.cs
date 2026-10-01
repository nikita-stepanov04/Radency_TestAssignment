namespace Radency_TestAssignment.Application.DTOs
{
    public class SaveUnitDTO
    {
        public int ID { get; set; }
        public int PropertyID { get; set; }
        public string Number { get; set; } = null!;
        public int Bedrooms { get; set; }
        public decimal MonthlyRent { get; set; }
        public int UnitTypeID { get; set; }
    }
}
