namespace Radency_TestAssignment.Application.DTOs
{
    public class SummaryDTO
    {
        public bool CanSubmit { get; set; }
        public List<string> BlockingReasons { get; set; } = new List<string>();
    }


}
