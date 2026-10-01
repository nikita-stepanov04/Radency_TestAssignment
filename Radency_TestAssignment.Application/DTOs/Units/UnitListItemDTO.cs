namespace Radency_TestAssignment.Application.DTOs
{
    public class UnitListItemDTO
    {
        public int ID { get; set; }
        public int PropertyID { get; set; }
        public string UnitNumber { get; set; } = string.Empty;
        public string UnitType { get; set; } = string.Empty;
        public int Bedrooms { get; set; }
        public decimal MonthlyRent { get; set; }
        public bool IsAvailable { get; set; }
    }
}
