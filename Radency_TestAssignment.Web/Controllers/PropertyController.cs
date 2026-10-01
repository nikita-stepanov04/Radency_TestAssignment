using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Radency_TestAssignment.Application.DTOs;
using Radency_TestAssignment.Application.IBusinessServices.Properties;
using Radency_TestAssignment.Web.Components;
using Radency_TestAssignment.Web.Identity;
using Radency_TestAssignment.Web.Models.Properties;
using Radency_TestAssignment.Web.Models.Shared;

namespace Radency_TestAssignment.Web.Controllers
{
    [Authorize(Policy = Policies.AuthorizedManagers)]
    public class PropertyController(
        IMapper _mapper,
        IPropertyBS _propertyBS) : Radency_TestAssignmentControllerBase
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult List(int page = 1)
            => ViewComponent(typeof(PropertiesListViewComponent), new { page });

        [HttpGet]
        public IActionResult Create() => PartialView("PropertyForm", CreateAddModal());

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var prop = await _propertyBS.GetByIdAsync(id);
            var viewModel = _mapper.Map<PropertyFormViewModel>(prop);

            var modal = CreateUpdateModal();
            modal.FormModel = viewModel;

            return PartialView("PropertyForm", modal);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save([Bind(Prefix = ModalBindingPrefix)] PropertyFormViewModel viewModel)
        {
            bool isNew = viewModel.ID == null;

            if (!ModelState.IsValid)
            {
                var modalModel = isNew ? CreateAddModal() : CreateUpdateModal();
                modalModel.FormModel = viewModel;
                return PartialView("PropertyForm", modalModel);
            }

            var dto = _mapper.Map<SavePropertyDTO>(viewModel);
            if (isNew) await _propertyBS.AddAsync(dto);
            else await _propertyBS.UpdateAsync(dto);

            return Json(new FormResult
            { 
                Message = $"Property {viewModel.Name} was successfully {(isNew ? "added" : "updated")}"
            });
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var dto = await _propertyBS.GetByIdAsync(id);
            if (dto is null) return NotFound();

            var modal = CreateDeleteModal();
            modal.FormModel = new DeleteItemViewModel { ID = dto.ID, Name = dto.Name };
            return PartialView("Forms/Modals/DeleteItem", modal);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete([Bind(Prefix = ModalBindingPrefix)] DeleteItemViewModel viewModel)
        {
            await _propertyBS.DeleteAsync(viewModel.ID);
            return Json(new FormResult { Message = $"Property {viewModel.Name} was deleted" });
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var dto = await _propertyBS.GetByIdAsync(id);
            if (dto is null) return NotFound();
            return View(_mapper.Map<PropertyFormViewModel>(dto));
        }

        private ModalFormViewModel<PropertyFormViewModel> CreateAddModal() =>
            new ModalFormViewModel<PropertyFormViewModel>()
            {
                Controller = "Property",
                Action = "Save",
                Title = "Add Property"
            };

        private ModalFormViewModel<PropertyFormViewModel> CreateUpdateModal() =>
            new ModalFormViewModel<PropertyFormViewModel>()
            {
                Controller = "Property",
                Action = "Save",
                Title = "Update Property"
            };

        private ModalFormViewModel<DeleteItemViewModel> CreateDeleteModal() =>
            new ModalFormViewModel<DeleteItemViewModel>()
            {
                Controller = "Property",
                Action = "Delete",
                IsDanger = true,
                Title = "Delete Property",
                SubmitText = "Delete"
            };
    }
}
