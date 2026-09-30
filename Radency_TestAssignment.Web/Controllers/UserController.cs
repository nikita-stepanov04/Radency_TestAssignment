using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Radency_TestAssignment.Application.DTOs;
using Radency_TestAssignment.Application.IBusinessServices.Users;
using Radency_TestAssignment.Web.Models.Users;

namespace Radency_TestAssignment.Web.Controllers
{
    public class UserController(
        IAccountBS _accountBS,
        IMapper _mapper): Radency_TestAssignmentControllerBase
    {
        [HttpGet]
        public IActionResult Login()
        {
            return View(new LogInViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Login(LogInViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var res = await _accountBS.LoginAsync(model.Email, model.Password);

            if (res == LogInStatus.InvalidEmail)
            {
                ModelState.AddModelError(nameof(LogInViewModel.Email), "Email was not found");
                return View(model);
            }
            else if (res == LogInStatus.InvalidPassword)
            {
                ModelState.AddModelError(nameof(LogInViewModel.Password), "Incorrect password");
                return View(model);
            }
            else return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> Registration()
        {
            var roles = await _accountBS.GetAllRolesAsync();

            var model = new RegistrationViewModel();
            model.SetRoles(roles);

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Registration(RegistrationViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var res = await _accountBS.RegisterAsync(_mapper.Map<RegistrationDTO>(model));

            if (res.HasError)
            {
                AlertWarning(res.ErrorMessage);

                var roles = await _accountBS.GetAllRolesAsync();
                model.SetRoles(roles);

                return View(model);
            }
            else return RedirectToAction("Index", "Home");
        }
    }
}
