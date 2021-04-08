using System;
using System.Threading.Tasks;
using FittShop.Domain.Dto;
using FittShop.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FittShop.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly LoginViewDto _loginViewDto;

        public AccountController(UserManager<IdentityUser> userMgr, SignInManager<IdentityUser> signInMng, LoginViewDto loginViewDto)
        {
            _userManager = userMgr;
            _signInManager = signInMng;
            _loginViewDto = loginViewDto;
        }

        [AllowAnonymous]
        public IActionResult Login(string returnUrl)
        {
            ViewBag.returnUrl = returnUrl;
            _loginViewDto.LoginModel = new LoginViewModel();
            return View(_loginViewDto);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewDto model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                IdentityUser user = await _userManager.FindByNameAsync(model.LoginModel.UserName);
                if (user is not null)
                {
                    await _signInManager.SignOutAsync(); //так нада
                    Microsoft.AspNetCore.Identity.SignInResult result =
                        await _signInManager.PasswordSignInAsync(user, model.LoginModel.Password, model.LoginModel.Remember, false);
                    if (result.Succeeded)
                    {
                        return Redirect(returnUrl ?? "/");
                    }
                }
                ModelState.AddModelError(nameof(LoginViewModel.UserName), "Неправильний логін або пароль");
            }
            return View(_loginViewDto);
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}