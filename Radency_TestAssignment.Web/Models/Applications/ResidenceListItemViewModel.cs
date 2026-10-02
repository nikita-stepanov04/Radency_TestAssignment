namespace Radency_TestAssignment.Web.Models.Applications
{
    public class ResidenceListItemViewModel
    {
        public int ID { get; set; }
        public string? Address { get; set; }
        public string? LandlordName { get; set; }
        public string? LandlordPhone { get; set; }
        public DateOnly? MoveInDate { get; set; }
        public DateOnly? MoveOutDate { get; set; }
    }
}
