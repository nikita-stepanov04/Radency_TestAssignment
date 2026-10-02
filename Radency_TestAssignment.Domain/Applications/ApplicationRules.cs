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

        public static ApplicationStep Next(ApplicationStep step)
        {
            return step == ApplicationStep.Summary ? step : step + 1;
        }

        public static ApplicationStep Previous(ApplicationStep step)
        {
            return step == ApplicationStep.ApplicantInfo ? step : step - 1;
        }

        public static bool CanWithdraw(ApplicationStatus status)
        {
            return status == ApplicationStatus.Draft
                || status == ApplicationStatus.Submitted
                || status == ApplicationStatus.Returned;
        }

        public static List<string> ValidateResidences(IReadOnlyCollection<ResidenceHistory> residences)
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
    }
}
