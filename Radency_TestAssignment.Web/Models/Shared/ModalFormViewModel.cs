namespace Radency_TestAssignment.Web.Models.Shared
{
    public class ModalFormViewModel<TFormModel>
    {
        public string Controller { get; set; } = null!;
        public string Action { get; set; } = null!;
        public string Title { get; set; } = "Form";
        public string SubmitText { get; set; } = "Save";
        public bool IsDanger { get; set; }

        public TFormModel FormModel { get; set; } = default!;
    }

}
