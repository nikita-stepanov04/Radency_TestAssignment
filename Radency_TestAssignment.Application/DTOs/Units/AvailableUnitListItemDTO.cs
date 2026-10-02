namespace Radency_TestAssignment.Application.DTOs
{
    public class AvailableUnitListItemDTO
    {
        public int ID { get; set; }
        public string PropertyName { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string UnitNumber { get; set; } = string.Empty;
        public string UnitType { get; set; } = string.Empty;
        public int Bedrooms { get; set; }
        public decimal MonthlyRent { get; set; }
        public int? ExistingApplicationId { get; set; }
    }
}
