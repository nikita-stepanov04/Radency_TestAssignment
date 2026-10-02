using System.ComponentModel.DataAnnotations;

namespace Radency_TestAssignment.Web.Models
{
    public class ResidenceFormViewModel : IValidatableObject
    {
        public int ID { get; set; }
        public int ApplicationID { get; set; }

        [Required, StringLength(500)]
        public string? Address { get; set; }

        [Required, StringLength(200), Display(Name = "Landlord name")]
        public string? LandlordName { get; set; }

        [Required, Phone, Display(Name = "Landlord phone")]
        public string? LandlordPhone { get; set; }

        [Required, DataType(DataType.Date), Display(Name = "Move in")]
        public DateOnly? MoveInDate { get; set; }

        [Required, DataType(DataType.Date), Display(Name = "Move out")]
        public DateOnly? MoveOutDate { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (MoveInDate.HasValue && MoveOutDate.HasValue && MoveOutDate < MoveInDate)
            {
                yield return new ValidationResult(
                    "Move-out date must not be before move-in date.",
                    new[] { nameof(MoveOutDate) });
            }
        }
    }
}
