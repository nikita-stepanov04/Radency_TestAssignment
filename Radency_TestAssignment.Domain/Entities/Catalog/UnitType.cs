namespace Radency_TestAssignment.Domain.Entities.Catalog
{
    public class UnitType : EntityBase
    {
        public string Name { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;

        public List<Unit> Units { get; set; } = new List<Unit>();
    }
}
