using Radency_TestAssignment.Domain.Entities.Applications;
using Radency_TestAssignment.Domain.Entities.Leasing;
using Radency_TestAssignment.Domain.Enums;

namespace Radency_TestAssignment.Domain.Applications
{
    public static class ApplicationRules
    {
        public static bool CanEdit(ApplicationStatus status)
        {
            return status == ApplicationStatus.Draft || status == ApplicationStatus.Returned;
        }

        public static bool AllSectionsSaved(IEnumerable<ApplicationSectionState> states)
        {
            var list = states.ToList();
            return list.Count > 0 && list.All(s => s.IsSaved);
        }

        public static bool HasActiveLease(IEnumerable<Lease> leases, DateOnly today)
        {
            return leases.Any(l => l.StartDate <= today && today <= l.EndDate);
        }

        public static ApplicationSection Next(ApplicationSection step)
        {
            return step == ApplicationSection.Summary ? step : step + 1;
        }

        public static ApplicationSection Previous(ApplicationSection step)
        {
            return step == ApplicationSection.ApplicantInformation ? step : step - 1;
        }

        public static bool CanWithdraw(ApplicationStatus status)
        {
            return status == ApplicationStatus.Draft
                || status == ApplicationStatus.Submitted
                || status == ApplicationStatus.Returned;
        }

        public static List<string> ValidateResidences(List<ResidenceHistory> residences)
        {
            var errors = new List<string>();

            if (residences.Count == 0)
            {
                errors.Add("Add at least one residence.");
                return errors;
            }

            var index = 0;
            foreach (var r in residences)
            {
                index++;

                if (string.IsNullOrWhiteSpace(r.Address)
                    || string.IsNullOrWhiteSpace(r.LandlordName)
                    || string.IsNullOrWhiteSpace(r.LandlordPhone)
                    || r.MoveInDate == null
                    || r.MoveOutDate == null)
                {
                    errors.Add($"Residence #{index} is incomplete.");
                }
                else if (r.MoveOutDate < r.MoveInDate)
                {
                    errors.Add($"Residence #{index}: move-out date is before move-in date.");
                }
            }

            return errors;
        }

        public static bool CanReview(ApplicationStatus status)
        {
            return status == ApplicationStatus.Submitted;
        }

        public static string? ValidateReview(ReviewOutcome outcome, string? comment)
        {
            if (outcome != ReviewOutcome.Approve && string.IsNullOrWhiteSpace(comment))
                return "A comment is required to return or deny an application.";

            return null;
        }

        public static ApplicationStatus GetStatus(ReviewOutcome outcome)
        {
            return outcome switch
            {
                ReviewOutcome.Approve => ApplicationStatus.Approved,
                ReviewOutcome.Return => ApplicationStatus.Returned,
                ReviewOutcome.Deny => ApplicationStatus.Denied,
                _ => throw new ArgumentOutOfRangeException(nameof(outcome))
            };
        }

        public static DateOnly GetLeaseEnd(DateOnly start)
        {
            return start.AddMonths(12).AddDays(-1);
        }
    }
}
