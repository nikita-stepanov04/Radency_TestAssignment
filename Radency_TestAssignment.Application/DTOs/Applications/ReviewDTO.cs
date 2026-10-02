using Radency_TestAssignment.Domain.Enums;

namespace Radency_TestAssignment.Application.DTOs
{
    public class ReviewDTO
    {
        public int ApplicationID { get; set; }
        public ReviewOutcome Outcome { get; set; }
        public string? Comment { get; set; }
    }
}
