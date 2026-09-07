using Microsoft.AspNetCore.Mvc;

namespace HelpDeskWeb.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
