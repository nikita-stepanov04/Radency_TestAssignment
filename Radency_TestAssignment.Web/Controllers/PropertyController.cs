using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Radency_TestAssignment.Application.DTOs;
using Radency_TestAssignment.Application.IBusinessServices.Properties;
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
        public IActionResult Create() => PartialView("PropertyForm", CreateModalModel());

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind(Prefix = nameof(ModalFormViewModel<>.FormModel))] PropertyFormViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                var modalModel = CreateModalModel();
                modalModel.FormModel = viewModel;
                return PartialView("PropertyForm", modalModel);
            }
            await _propertyBS.AddAsync(_mapper.Map<AddPropertyDTO>(viewModel));

            return Json(new FormResult { Message = $"Property {viewModel.Name} was successfully added" });
        }

        private ModalFormViewModel<PropertyFormViewModel> CreateModalModel() =>
            new ModalFormViewModel<PropertyFormViewModel>()
            {
                Controller = "Property",
                Action = "Create",
                Title = "Add Property"
            };
    }
}
