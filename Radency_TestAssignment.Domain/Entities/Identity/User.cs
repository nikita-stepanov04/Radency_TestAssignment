using Microsoft.AspNetCore.Identity;
using Radency_TestAssignment.Domain.Entities.Applications;

namespace Radency_TestAssignment.Domain.Entities.Identity
{
    public class User : IdentityUser<int>
    {
        public string FullName { get; set; } = string.Empty;

        public List<RentalApplication> Applications { get; set; } = new List<RentalApplication>();
    }
}
