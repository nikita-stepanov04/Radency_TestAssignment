using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Radency_TestAssignment.Application.DTOs;
using Radency_TestAssignment.Application.IBusinessServices;
using Radency_TestAssignment.Domain.Entities.Identity;
using Radency_TestAssignment.Domain.Enums;
using System.Security.Claims;

namespace Radency_TestAssignment.Web.Components
{
    public class ApplicationListViewComponent(
    IApplicationBS _applicationBS,
    IOptions<PaginationSettings> _pagOpts) : ViewComponent
    {
        private readonly PaginationSettings _paginationSettings = _pagOpts.Value;

        public async Task<IViewComponentResult> InvokeAsync(
            ApplicationStatus? status = null, int? propertyID = null, int page = 1)
        {
            var isManager = UserClaimsPrincipal.IsInRole(RoleNames.PropertyManager);
            var userID = int.Parse(UserClaimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var filter = new ApplicationListFilterDTO
            {
                Status = status,
                PropertyID = propertyID,
                Page = page,
                PageSize = _paginationSettings.ItemsPerPage
            };

            var result = await _applicationBS.GetListAsync(filter, userID, isManager);

            ViewData["Status"] = status;
            ViewData["PropertyID"] = propertyID;
            ViewData["IsManager"] = isManager;
            return View(result);
        }
    }
}
