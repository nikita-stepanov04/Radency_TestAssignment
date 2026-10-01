namespace Radency_TestAssignment.Domain.Entities.Catalog
{
    public class UnitType : EntityBase
    {
        public UnitType() { }
        public UnitType(string name, bool isActive) 
        {
            Name = name;
            IsActive = isActive;
        }

        public string Name { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;

        public List<Unit> Units { get; set; } = new List<Unit>();
    }
}
