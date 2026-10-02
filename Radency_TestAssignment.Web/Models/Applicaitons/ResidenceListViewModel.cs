namespace Radency_TestAssignment.Web.Models.Applications
{
    public class ResidenceListViewModel
    {
        public int ApplicationID { get; set; }
        public bool IsReadOnly { get; set; }
        public List<ResidenceListItemViewModel> Items { get; set; } = new List<ResidenceListItemViewModel>();
    }
}
