using System.Threading.Tasks;
using FittShop.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace FittShop.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;

        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login(string returnUrl)
        {
            ViewBag.returnUrl = returnUrl;
            return View(new LoginModel());
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromForm] LoginModel model, string returnUrl)
        {
            if (!ModelState.IsValid) 
                return View(model);
            
            IdentityUser user = await this.userManager.FindByNameAsync(model.UserName);
            
            if (user is not null)
            {
                await this.signInManager.SignOutAsync(); //так нада
                SignInResult result = await this.signInManager.PasswordSignInAsync(user, model.Password, model.Remember, false);
                
                if (result.Succeeded) 
                    return this.LocalRedirect(returnUrl ?? "/");
            }
            
            ModelState.AddModelError(nameof(LoginModel.UserName), "Неправильний логін або пароль");

            return View(model);
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await this.signInManager.SignOutAsync();
            return RedirectToAction("all", "products");
        }
    }
}