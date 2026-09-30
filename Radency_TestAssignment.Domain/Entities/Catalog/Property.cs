namespace Radency_TestAssignment.Domain.Entities.Catalog
{
    public class Property : EntityBase
    {
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;

        public List<Unit> Units { get; set; } = new List<Unit>();
    }
}
