using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Radency_TestAssignment.Application.DTOs;
using Radency_TestAssignment.Application.IBusinessServices;
using Radency_TestAssignment.Web.Identity;
using Radency_TestAssignment.Web.Models;
using Radency_TestAssignment.Web.Models.Shared;

namespace Radency_TestAssignment.Web.Controllers
{
    [Authorize(Policy = Policies.AuthorizedAny)]
    public class ResidenceHistoryController(
        IMapper _mapper,
        IResidenceHistoryBS _residenceBS,
        IApplicationBS _applicationBS) : Radency_TestAssignmentControllerBase
    {
        private const string FormView = "ResidenceForm";

        [HttpGet]
        public async Task<IActionResult> List(int applicationID)
        {
            var access = await _applicationBS.GetAccessAsync(applicationID, UserID);
            if (access == null) return NotFound();

            return ViewComponent("ResidenceList", new
            {
                applicationID,
                isReadOnly = !access.IsEditable
            });
        }

        [HttpGet]
        public async Task<IActionResult> Create(int applicationID)
        {
            var access = await _applicationBS.GetAccessAsync(applicationID, UserID);
            if (access == null) return NotFound();
            if (!access.IsEditable) return StatusCode(StatusCodes.Status409Conflict);

            var form = new ResidenceFormViewModel { ApplicationID = applicationID };
            return PartialView(FormView, CreateModal("Create", "Add residence", "Add", form));
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind(Prefix = ModalBindingPrefix)] ResidenceFormViewModel model)
        {
            var modal = CreateModal("Create", "Add residence", "Add", model);

            if (!ModelState.IsValid)
                return PartialView(FormView, modal);

            var result = await _residenceBS.CreateAsync(UserID, _mapper.Map<ResidenceDTO>(model));
            if (result.HasError)
            {
                return Json(new FormResult { Message = result.ErrorMessage, Type = "danger" });
            }

            return Json(new FormResult { Message = "Residence was successfully added" });
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var result = await _residenceBS.GetForEditAsync(id, UserID);
            if (result.HasError) return NotFound(result.ErrorMessage);

            var form = _mapper.Map<ResidenceFormViewModel>(result.Result);
            return PartialView(FormView, CreateModal("Edit", "Edit residence", "Save", form));
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind(Prefix = ModalBindingPrefix)] ResidenceFormViewModel model)
        {
            var modal = CreateModal("Edit", "Edit residence", "Save", model);

            if (!ModelState.IsValid)
                return PartialView(FormView, modal);

            var result = await _residenceBS.UpdateAsync(UserID, _mapper.Map<ResidenceDTO>(model));
            if (result.HasError)
            {
                return Json(new FormResult { Message = result.ErrorMessage, Type = "danger" });
            }

            return Json(new FormResult { Message = $"Residence {model.Address} was successfully updated" });
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _residenceBS.GetForEditAsync(id, UserID);
            if (result.HasError) return NotFound();

            var modal = new ModalFormViewModel<DeleteItemViewModel>()
            {
                Controller = "ResidenceHistory",
                Action = "Delete",
                IsDanger = true,
                Title = "Delete Residence",
                SubmitText = "Delete"
            };
            modal.FormModel = new DeleteItemViewModel { ID = result.Result.ID, Name = result.Result.Address };
            return PartialView("Forms/Modals/DeleteItem", modal);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete([Bind(Prefix = ModalBindingPrefix)] DeleteItemViewModel viewModel)
        {
            var result = await _residenceBS.DeleteAsync(UserID, viewModel.ID);
            if (result.HasError) return BadRequest(result.ErrorMessage);

            return Json(new FormResult { Message = $"Residence {viewModel.Name} was deleted" });
        }

        private static ModalFormViewModel<ResidenceFormViewModel> CreateModal(
            string action,
            string title,
            string submitText,
            ResidenceFormViewModel form,
            bool isDanger = false)
        {
            return new ModalFormViewModel<ResidenceFormViewModel>
            {
                Controller = "ResidenceHistory",
                Action = action,
                Title = title,
                SubmitText = submitText,
                IsDanger = isDanger,
                FormModel = form
            };
        }            
    }
}
