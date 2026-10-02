using System.ComponentModel.DataAnnotations;

namespace Radency_TestAssignment.Web.Models.Applications
{
    public class ApplicantInfoSectionViewModel
    {
        public Guid Version { get; set; }
        public bool IsReadOnly { get; set; }

        [Required, StringLength(200)]
        public string? FullName { get; set; }

        [Required, Phone]
        public string? Phone { get; set; }

        [Required, EmailAddress]
        public string? Email { get; set; }

        [Required, StringLength(500)]
        public string? CurrentAddress { get; set; }
    }
}
