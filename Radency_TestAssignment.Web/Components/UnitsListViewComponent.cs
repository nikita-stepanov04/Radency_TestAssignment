using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Radency_TestAssignment.Application.IBusinessServices;
using Radency_TestAssignment.Domain.Pagination;

namespace Radency_TestAssignment.Web.Components
{
    public class UnitsListViewComponent(
        IUnitBS _unitBS,
        IOptions<PaginationSettings> _pagOpts) : ViewComponent
    {
        private readonly PaginationSettings _paginationSettings = _pagOpts.Value;

        public async Task<IViewComponentResult> InvokeAsync(int propertyId, int page = 1)
        {
            ViewData["PropertyId"] = propertyId;
            return View(await _unitBS.GetPagedAsync(propertyId, new PageRequest(page, _paginationSettings.ItemsPerPage)));
        }
    }
}
