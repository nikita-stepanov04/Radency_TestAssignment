using Radency_TestAssignment.Domain.Enums;

namespace Radency_TestAssignment.Application.DTOs
{
    public class ApplicationListFilterDTO
    {
        public ApplicationStatus? Status { get; set; }
        public int? PropertyID { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; }
    }
}
