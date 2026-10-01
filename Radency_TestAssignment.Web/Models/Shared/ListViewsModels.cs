namespace Radency_TestAssignment.Web.Models.Shared
{
    public record PagerViewModel(int Page, int TotalPages, Func<int, string?> PageUrl);

    public record RowActionsViewModel(string EditUrl, string DeleteUrl, string? DetailsUrl = null);
}
