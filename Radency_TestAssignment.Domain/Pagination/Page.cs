namespace Radency_TestAssignment.Domain.Pagination
{
    public record PageRequest(int Page = 1, int PageSize = 20)
    {
        public int Skip => (Math.Max(Page, 1) - 1) * PageSize;
    }

    public record PagedResult<T>(IEnumerable<T> Items, int TotalCount, int Page, int PageSize)
    {
        public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);
    }
}
