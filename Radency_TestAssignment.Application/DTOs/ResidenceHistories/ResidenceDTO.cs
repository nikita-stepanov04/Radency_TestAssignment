namespace Radency_TestAssignment.Application.DTOs
{
    public class ResidenceDTO
    {
        public int ID { get; set; }
        public int ApplicationID { get; set; }
        public string Address { get; set; } = string.Empty;
        public string LandlordName { get; set; } = string.Empty;
        public string LandlordPhone { get; set; } = string.Empty;
        public DateOnly MoveInDate { get; set; }
        public DateOnly MoveOutDate { get; set; }
    }
}
