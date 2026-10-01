using System.ComponentModel.DataAnnotations;

namespace Radency_TestAssignment.Web
{
    public class PaginationSettings
    {
        [Required, DeniedValues(0)]
        public int ItemsPerPage { get; set; }
    }
}
