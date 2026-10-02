using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Radency_TestAssignment.Application;
using Radency_TestAssignment.Application.DTOs;
using Radency_TestAssignment.Application.IBusinessServices;
using Radency_TestAssignment.Domain.Applications;
using Radency_TestAssignment.Web.Models.Applications;
using Radency_TestAssignment.Web.Models.Shared;

namespace Radency_TestAssignment.Web.Controllers
{
    public class ApplicationController(
        IMapper _mapper,
        IApplicationBS _applicationBS) : Radency_TestAssignmentControllerBase
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Start(int unitID)
        {
            var result = await _applicationBS.StartAsync(unitID, UserID);
            if (result.HasError)
            {
                AlertDanger(result.ErrorMessage);
                return RedirectToAction("Index", "Units");
            }

            return RedirectToAction(nameof(Edit), new { id = result.Result });
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id, ApplicationStep step = ApplicationStep.ApplicantInfo)
        {
            var modelDTO = await _applicationBS.GetWizardAsync(id, UserID, step);
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

        private async Task<IActionResult> ContinueAsync(ApplicationWizardViewModel model)
        {
            var userID = UserID;
            OpRes<bool> result;

            ModelState.Clear();

            switch (model.Step)
            {
                case ApplicationStep.ApplicantInfo:
                    if (!TryValidateModel(model.ApplicantInformation, nameof(model.ApplicantInformation)))
                    {
                        return await RenderInvalidAsync(model);
                    }
                    var applicantInfoDTO = _mapper.Map<ApplicantInfoSectionDTO>(model.ApplicantInformation);
                    result = await _applicationBS.SaveApplicantInfoAsync(model.ID, userID, applicantInfoDTO);
                    break;

                case ApplicationStep.ResidenceHistory:
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

        private async Task<IActionResult> RenderAsync(int id, ApplicationStep step)
        {
            var wizardDTO = await _applicationBS.GetWizardAsync(id, UserID, step);
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
    }
}
