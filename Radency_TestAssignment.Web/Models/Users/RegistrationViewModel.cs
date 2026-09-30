using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Radency_TestAssignment.Web.Models.Users
{
    public class RegistrationViewModel
    {
        [Required(ErrorMessage = "Enter an email address")]
        [EmailAddress(ErrorMessage = "Enter a valid email address")]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email", Prompt = "Enter your email")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Enter your full name")]
        [StringLength(50, ErrorMessage = "Full name must be between {2} and {1} characters long", MinimumLength = 2)]
        [Display(Name = "Full Name", Prompt = "Enter your full name")]
        public string FullName { get; set; } = null!;

        [Required(ErrorMessage = "Enter password")]
        [StringLength(8, ErrorMessage = "Password must be between {2} and {1} characters long", MinimumLength = 4)]
        [DataType(DataType.Password)]
        [Display(Name = "Password", Prompt = "Enter your password")]
        public string Password { get; set; } = null!;

        [Required(ErrorMessage = "Select a role")]
        [Display(Name = "Role")]
        public string? Role { get; set; }

        [ValidateNever]
        public List<SelectListItem> Roles { get; set; } = [];

        public void SetRoles(IEnumerable<string> roles)
            => Roles = roles.Select(r => new SelectListItem(r, r)).ToList();
    }

}
