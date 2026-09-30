namespace Radency_TestAssignment.Web.Models.Shared
{
    public class RedirectLinkViewModel
    {
        public RedirectLinkViewModel() { }
        public RedirectLinkViewModel(string text, string path) 
        { 
            Text = text;
            Path = path;
        }

        public string Text { get; set; } = null!;
        public string Path { get; set; } = null!;
    }
}
