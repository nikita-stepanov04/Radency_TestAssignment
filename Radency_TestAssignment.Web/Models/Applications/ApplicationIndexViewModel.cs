using Microsoft.AspNetCore.Mvc.Rendering;
using Radency_TestAssignment.Domain.Enums;

namespace Radency_TestAssignment.Web.Models
{
    public class ApplicationIndexViewModel
    {
        public List<SelectListItem> Statuses { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> Properties { get; set; } = new List<SelectListItem>();
        public ApplicationStatus? Status { get; init; }
        public int? PropertyID { get; init; }
    }
}
