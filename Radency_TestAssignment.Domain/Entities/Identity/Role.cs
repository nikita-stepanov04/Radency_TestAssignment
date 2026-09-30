using Microsoft.AspNetCore.Identity;

namespace Radency_TestAssignment.Domain.Entities.Identity
{
    public class Role : IdentityRole<int>
    {
        public Role() { }

        public Role(string roleName) : base(roleName) { }
    }
}
