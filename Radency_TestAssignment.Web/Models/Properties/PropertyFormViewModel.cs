using System.ComponentModel.DataAnnotations;

namespace Radency_TestAssignment.Web.Models.Properties
{
    public class PropertyFormViewModel
    {
        [Required(ErrorMessage = "Enter property name")]
        [StringLength(200, ErrorMessage = "Property name must be between {2} and {1} characters long", MinimumLength = 2)]
        [Display(Name = "Property Name", Prompt = "Enter property name")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "Enter property address")]
        [StringLength(300, ErrorMessage = "Address name must be between {2} and {1} characters long", MinimumLength = 2)]
        [Display(Name = "Address", Prompt = "Enter property address")]
        public string Address { get; set; } = null!;

        [Required(ErrorMessage = "Enter city")]
        [StringLength(100, ErrorMessage = "City name must be between {2} and {1} characters long", MinimumLength = 2)]
        [Display(Name = "City", Prompt = "Enter city")]
        public string City { get; set; } = null!;
    }
}
