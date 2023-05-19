using HomeDatabase.Models;
using Microsoft.AspNetCore.Mvc;

namespace HomeDatabase.Controllers
{
    public class AcountController : Controller
    {
        public IActionResult AcountView()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LogInViewModel login)
        {

            return RedirectToAction("Databases", "Databases");
        }
    }
}
