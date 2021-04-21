using Microsoft.AspNetCore.Mvc;

namespace FittShop.Controllers
{
    public class InfoController : Controller
    {
        [HttpGet]
        public IActionResult Contacts()
        {
            return View();
        }
    }
}