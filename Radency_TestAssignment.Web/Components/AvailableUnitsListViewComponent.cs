using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Radency_TestAssignment.Application.IBusinessServices;
using Radency_TestAssignment.Domain.Pagination;
using System.Security.Claims;

namespace Radency_TestAssignment.Web.Components
{
    public class AvailableUnitsListViewComponent(
        IUnitBS _unitBS,
        IOptions<PaginationSettings> _pagOpts) : ViewComponent
    {
        private readonly PaginationSettings _paginationSettings = _pagOpts.Value;

        public async Task<IViewComponentResult> InvokeAsync(int page = 1)
        {
            var userId = int.Parse(UserClaimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var result = await _unitBS.GetAvailablePagedAsync(
                userId, new PageRequest(page, _paginationSettings.ItemsPerPage));

            return View(result);
        }
    }
}
