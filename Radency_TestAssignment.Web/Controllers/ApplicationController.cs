using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Radency_TestAssignment.Application;
using Radency_TestAssignment.Application.DTOs;
using Radency_TestAssignment.Application.IBusinessServices;
using Radency_TestAssignment.Application.IBusinessServices.Properties;
using Radency_TestAssignment.Domain.Applications;
using Radency_TestAssignment.Domain.Entities.Identity;
using Radency_TestAssignment.Domain.Enums;
using Radency_TestAssignment.Web.Components;
using Radency_TestAssignment.Web.Identity;
using Radency_TestAssignment.Web.Models;
using Radency_TestAssignment.Web.Models.Applications;
using Radency_TestAssignment.Web.Models.Shared;

namespace Radency_TestAssignment.Web.Controllers
{
    [Authorize(Policy = Policies.AuthorizedAny)]
    public class ApplicationController(
        IMapper _mapper,
        IPropertyBS _propertyBS,
        IApplicationBS _applicationBS) : Radency_TestAssignmentControllerBase
    {
        private bool IsManager => User.IsInRole(RoleNames.PropertyManager);

        [HttpGet]
        public async Task<IActionResult> Index(ApplicationStatus? status, int? propertyID)
        {
            var isManager = User.IsInRole(RoleNames.PropertyManager);
            var properties = await _propertyBS.GetLookupAsync();

            var statuses = Enum.GetValues<ApplicationStatus>()
                .Where(s => !isManager || s != ApplicationStatus.Draft)
                .Select(s => new SelectListItem(s.ToString(), s.ToString(), s == status))
                .ToList();

            return View(new ApplicationIndexViewModel
            {
                Statuses = statuses,
                Properties = properties
                    .Select(p => new SelectListItem(p.Name, p.ID.ToString(), p.ID == propertyID))
                    .ToList(),
                Status = status,
                PropertyID = propertyID
            });
        }

        [HttpGet]
        public IActionResult List(ApplicationStatus? status, int? propertyID, int page = 1)
            => ViewComponent(typeof(ApplicationListViewComponent), new { status, propertyID, page });

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Start(int unitID)
        {
            var result = await _applicationBS.StartAsync(unitID, UserID);
            if (result.HasError)
            {
                AlertDanger(result.ErrorMessage);
                return RedirectToAction("Index", "Units");
            }

            return RedirectToAction(nameof(Edit), new 
            { 
                id = result.Result, 
                step = ApplicationSection.ApplicantInformation 
            });
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id, ApplicationSection? step)
        {
            var modelDTO = await _applicationBS.GetWizardAsync(id, UserID, step, IsManager);
            if (modelDTO == null) return NotFound();

            var viewModel = _mapper.Map<ApplicationWizardViewModel>(modelDTO);
            return View(viewModel);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Step(ApplicationWizardViewModel model, string command)
        {
            var access = await _applicationBS.GetAccessAsync(model.ID, UserID);
            if (access == null) return NotFound();
            if (!access.IsEditable) return StatusCode(StatusCodes.Status409Conflict);

            switch (command)
            {
                case WizardCommands.Back:
                    ModelState.Clear();
                    return await RenderAsync(model.ID, ApplicationRules.Previous(model.Step));

                case WizardCommands.Continue: return await ContinueAsync(model);
                case WizardCommands.Submit: return await SubmitAsync(model);
                default: return BadRequest();
            }
        }

        [HttpGet]
        [Authorize(Policy = Policies.AuthorizedManagers)]
        public async Task<IActionResult> Review(int id)
        {
            var access = await _applicationBS.GetViewAccessAsync(id, UserID, isManager: true);
            if (access == null || !ApplicationRules.CanReview(access.Status)) return NotFound();

            return PartialView("ReviewForm", CreateReviewModal(new ReviewFormViewModel { ID = id }));
        }

        [HttpPost, ValidateAntiForgeryToken]
        [Authorize(Policy = Policies.AuthorizedManagers)]
        public async Task<IActionResult> Review([Bind(Prefix = ModalBindingPrefix)] ReviewFormViewModel model)
        {
            if (!ModelState.IsValid)
                return PartialView("ReviewForm", CreateReviewModal(model));

            var result = await _applicationBS.ReviewAsync(_mapper.Map<ReviewDTO>(model), UserID);
            if (result.HasError)
            {
                ModelState.AddModelError(string.Empty, result.ErrorMessage);
                return PartialView("ReviewForm", CreateReviewModal(model));
            }

            var message = model.Outcome switch
            {
                ReviewOutcome.Approve => "Application approved",
                ReviewOutcome.Return => "Application returned to the applicant",
                _ => "Application denied"
            };

            return Json(new FormResult { Message = message });
        }

        private async Task<IActionResult> ContinueAsync(ApplicationWizardViewModel model)
        {
            var userID = UserID;
            OpRes<bool> result;

            ModelState.Clear();

            switch (model.Step)
            {
                case ApplicationSection.ApplicantInformation:
                    if (!TryValidateModel(model.ApplicantInformation, nameof(model.ApplicantInformation)))
                    {
                        return await RenderInvalidAsync(model);
                    }
                    var applicantInfoDTO = _mapper.Map<ApplicantInfoSectionDTO>(model.ApplicantInformation);
                    result = await _applicationBS.SaveApplicantInfoAsync(model.ID, userID, applicantInfoDTO);
                    break;

                case ApplicationSection.ResidenceHistory:
                    result = await _applicationBS.SaveResidenceHistoryAsync(model.ID, userID, model.ResidenceHistory.Version);
                    break;

                default: return BadRequest();
            }

            if (result.HasError)
            {
                ModelState.AddModelError(string.Empty, result.ErrorMessage);
                return await RenderInvalidAsync(model);
            }

            ModelState.Clear();
            return await RenderAsync(model.ID, ApplicationRules.Next(model.Step));
        }

        private async Task<IActionResult> SubmitAsync(ApplicationWizardViewModel model)
        {
            var result = await _applicationBS.SubmitAsync(model.ID, UserID);
            if (result.HasError)
                return Json(new FormResult { Message = result.ErrorMessage, Type = "danger" });

            AlertSuccess("Application submitted");
            return Json(new { redirectUrl = Url.Action("Index", "Home") });
        }

        private async Task<IActionResult> RenderAsync(int id, ApplicationSection step)
        {
            var wizardDTO = await _applicationBS.GetWizardAsync(id, UserID, step, false);
            if (wizardDTO == null) return NotFound();

            var viewModel = _mapper.Map<ApplicationWizardViewModel>(wizardDTO);
            return PartialView("Wizard", viewModel);
        }

        private async Task<IActionResult> RenderInvalidAsync(ApplicationWizardViewModel posted)
        {
            var dto = _mapper.Map<ApplicationWizardDTO>(posted);
            await _applicationBS.FillContextAsync(dto, UserID);
            return PartialView("Wizard", posted);
        }

        private static ModalFormViewModel<ReviewFormViewModel> CreateReviewModal(ReviewFormViewModel form)
        {
            return new ModalFormViewModel<ReviewFormViewModel>
            {
                Controller = "Application",
                Action = "Review",
                Title = "Review application",
                SubmitText = "Save review",
                FormModel = form
            };
        }
    }
}
