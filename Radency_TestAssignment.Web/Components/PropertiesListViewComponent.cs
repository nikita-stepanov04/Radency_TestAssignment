using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Radency_TestAssignment.Application.IBusinessServices.Properties;
using Radency_TestAssignment.Domain.Pagination;

namespace Radency_TestAssignment.Web.Components
{
    public class PropertiesListViewComponent(
        IPropertyBS _propertyBS,
        IOptions<PaginationSettings> _pagOpts) : ViewComponent
    {
        private readonly PaginationSettings _paginationSettings = _pagOpts.Value;

        public async Task<IViewComponentResult> InvokeAsync(int page = 1)
        {
            var result = await _propertyBS.GetPagedAsync(new PageRequest(page, _paginationSettings.ItemsPerPage));
            return View(result);
        }
    }
}
