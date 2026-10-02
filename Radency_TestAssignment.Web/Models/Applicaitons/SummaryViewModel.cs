namespace Radency_TestAssignment.Web.Models.Applications
{
    public class SummaryViewModel
    {
        public bool CanSubmit { get; set; }
        public List<string> BlockingReasons { get; set; } = new List<string>();
    }
}
