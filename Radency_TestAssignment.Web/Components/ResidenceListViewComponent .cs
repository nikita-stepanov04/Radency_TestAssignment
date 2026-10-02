using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Radency_TestAssignment.Application.IBusinessServices;
using Radency_TestAssignment.Domain.Entities.Identity;
using Radency_TestAssignment.Web.Models.Applications;
using System.Security.Claims;

namespace Radency_TestAssignment.Web.Components
{
    public class ResidenceListViewComponent(
        IMapper _mapper,
        IResidenceHistoryBS _residenceBS) : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(int applicationID, bool isReadOnly)
        {
            var userId = int.Parse(UserClaimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var isManager = UserClaimsPrincipal.IsInRole(RoleNames.PropertyManager);
            var result = await _residenceBS.GetListAsync(applicationID, userId, isManager);

            if (result.HasError)
                return Content(string.Empty);

            return View(new ResidenceListViewModel
            {
                ApplicationID = applicationID,
                IsReadOnly = isReadOnly,
                Items = _mapper.Map<List<ResidenceListItemViewModel>>(result.Result)
            });
        }
    }
}
