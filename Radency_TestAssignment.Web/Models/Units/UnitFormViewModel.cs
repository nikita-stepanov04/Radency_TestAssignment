using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using Radency_TestAssignment.Domain.Entities.Catalog;
using System.ComponentModel.DataAnnotations;

namespace Radency_TestAssignment.Web.Models
{
    public class UnitFormViewModel
    {
        public int? ID { get; set; }
        public int? PropertyID { get; set; }

        [Required(ErrorMessage = "Enter unit number")]
        [StringLength(50, ErrorMessage = "Unit number must be between {2} and {1} characters long", MinimumLength = 1)]
        [Display(Name = "Unit Number", Prompt = "Enter unit number")]
        public string Number { get; set; } = null!;

        [Required(ErrorMessage = "Enter number of bedrooms")]
        [Range(0, 20, ErrorMessage = "Bedrooms must be between {1} and {2}")]
        [Display(Name = "Bedrooms", Prompt = "Enter number of bedrooms")]
        public int Bedrooms { get; set; }

        [Required(ErrorMessage = "Enter monthly rent")]
        [Range(0, 100000, ErrorMessage = "Monthly rent must be between {1} and {2}")]
        [Display(Name = "Monthly Rent", Prompt = "Enter monthly rent")]
        [DataType(DataType.Currency)]
        public decimal MonthlyRent { get; set; }

        [Required(ErrorMessage = "Select availability")]
        [Display(Name = "Available")]
        public bool IsAvailable { get; set; }

        [Required(ErrorMessage = "Select a unit type")]
        [Display(Name = "Unit Type")]
        public string? UnitTypeID { get; set; }

        [ValidateNever]
        public List<SelectListItem> UnitTypes { get; set; } = [];

        public void SetUnitTypes(IEnumerable<UnitType> unitTypes)
            => UnitTypes = unitTypes.Select(t => new SelectListItem(t.Name, t.ID.ToString())).ToList();
    }

}
