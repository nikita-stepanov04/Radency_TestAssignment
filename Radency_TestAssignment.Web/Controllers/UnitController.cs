using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Radency_TestAssignment.Application;
using Radency_TestAssignment.Application.DTOs;
using Radency_TestAssignment.Application.IBusinessServices;
using Radency_TestAssignment.Web.Components;
using Radency_TestAssignment.Web.Identity;
using Radency_TestAssignment.Web.Models;
using Radency_TestAssignment.Web.Models.Shared;

namespace Radency_TestAssignment.Web.Controllers
{
    [Authorize(Policy = Policies.AuthorizedManagers)]
    public class UnitController(
        IMapper _mapper,
        IUnitBS _unitBS) : Radency_TestAssignmentControllerBase
    {
        [HttpGet]
        public IActionResult List(int propertyId, int page = 1)
            => ViewComponent(typeof(UnitsListViewComponent), new { propertyId, page });

        [HttpGet]
        public async Task<IActionResult> Create(int propertyID)
        {
            var unitTypes = await _unitBS.GetUnitTypesAsync();

            var modal = CreateAddModal();
            modal.FormModel = new UnitFormViewModel();
            modal.FormModel.SetUnitTypes(unitTypes);
            modal.FormModel.PropertyID = propertyID;

            return PartialView("UnitForm", modal);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var unitTypes = await _unitBS.GetUnitTypesAsync();
            var unit = await _unitBS.GetByIdAsync(id);

            var viewModel = _mapper.Map<UnitFormViewModel>(unit);

            var modal = CreateUpdateModal();
            modal.FormModel = viewModel;
            modal.FormModel.SetUnitTypes(unitTypes);

            return PartialView("UnitForm", modal);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save([Bind(Prefix = ModalBindingPrefix)] UnitFormViewModel viewModel)
        {
            bool isNew = viewModel.ID == null;

            if (!ModelState.IsValid)
            {
                var modalModel = isNew ? CreateAddModal() : CreateUpdateModal();
                modalModel.FormModel = viewModel;
                return PartialView("PropertyForm", modalModel);
            }

            var dto = _mapper.Map<SaveUnitDTO>(viewModel);

            Task<OpRes<int>> resT = isNew 
                ? _unitBS.AddAsync(dto)
                : _unitBS.UpdateAsync(dto);

            var res = await resT;

            return Json(res.HasError
                ? new FormResult { Message = res.ErrorMessage, Type = "danger" }
                : new FormResult { Message = $"Unit {viewModel.Number} was successfully {(isNew ? "added" : "updated")}" });
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var dto = await _unitBS.GetByIdAsync(id);
            if (dto is null) return NotFound();

            var modal = CreateDeleteModal();
            modal.FormModel = new DeleteItemViewModel { ID = dto.ID, Name = dto.UnitNumber };
            return PartialView("Forms/Modals/DeleteItem", modal);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete([Bind(Prefix = ModalBindingPrefix)] DeleteItemViewModel viewModel)
        {
            await _unitBS.DeleteAsync(viewModel.ID);
            return Json(new FormResult { Message = $"Unit {viewModel.Name} was deleted" });
        }

        private ModalFormViewModel<UnitFormViewModel> CreateAddModal() =>
            new ModalFormViewModel<UnitFormViewModel>()
            {
                Controller = "Unit",
                Action = "Save",
                Title = "Add Unit"
            };

        private ModalFormViewModel<UnitFormViewModel> CreateUpdateModal() =>
            new ModalFormViewModel<UnitFormViewModel>()
            {
                Controller = "Unit",
                Action = "Save",
                Title = "Update Unit"
            };

        private ModalFormViewModel<DeleteItemViewModel> CreateDeleteModal() =>
            new ModalFormViewModel<DeleteItemViewModel>()
            {
                Controller = "Unit",
                Action = "Delete",
                IsDanger = true,
                Title = "Delete Unit",
                SubmitText = "Delete"
            };
    }
}
