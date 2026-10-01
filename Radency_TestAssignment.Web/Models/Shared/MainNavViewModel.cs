namespace Radency_TestAssignment.Web.Models.Shared
{
    public class MainNavViewModel
    {
        public bool IsAuthenticated { get; init; }
        public string? Email { get; init; }
        public bool IsManager { get; init; }
        public bool IsApplicant { get; init; }
    }
}
