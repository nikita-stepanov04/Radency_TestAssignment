using System.ComponentModel.DataAnnotations;

namespace Radency_TestAssignment.Web.Models.Users
{
    public class LogInViewModel
    {
        [Required(ErrorMessage = "Enter an email address")]
        [EmailAddress(ErrorMessage = "Enter a valid email address")]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email", Prompt = "Enter your email")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Enter password")]
        [StringLength(8, ErrorMessage = "Password must be between {2} and {1} characters long", MinimumLength = 4)]
        [DataType(DataType.Password)]
        [Display(Name = "Password", Prompt = "Enter your password")]
        public string Password { get; set; } = null!;

    }
}
