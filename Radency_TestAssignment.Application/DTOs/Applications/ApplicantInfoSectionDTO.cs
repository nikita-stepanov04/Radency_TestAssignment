namespace Radency_TestAssignment.Application.DTOs
{
    public class ApplicantInfoSectionDTO
    {
        public Guid Version { get; set; }
        public bool IsReadOnly { get; set; }
        public string? FullName { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? CurrentAddress { get; set; }
    }
}
