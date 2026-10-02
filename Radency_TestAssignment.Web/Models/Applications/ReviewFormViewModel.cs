using Microsoft.AspNetCore.Mvc.Rendering;
using Radency_TestAssignment.Domain.Applications;
using Radency_TestAssignment.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Radency_TestAssignment.Web.Models
{
    public class ReviewFormViewModel : IValidatableObject
    {
        public int ID { get; set; }

        [Required]
        public ReviewOutcome? Outcome { get; set; }

        [StringLength(2000)]
        public string? Comment { get; set; }

        public List<SelectListItem> Outcomes => Enum.GetValues<ReviewOutcome>()
            .Select(o => new SelectListItem(o.ToString(), o.ToString()))
            .ToList();

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!Outcome.HasValue) yield break;

            var error = ApplicationRules.ValidateReview(Outcome.Value, Comment);
            if (error != null)
                yield return new ValidationResult(error, new[] { nameof(Comment) });
        }
    }
}
