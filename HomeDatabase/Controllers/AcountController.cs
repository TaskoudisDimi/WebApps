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


        public IActionResult LogIn(LogInViewModel login)
        {
            return View();
        }

        public IActionResult Register(LogInViewModel login)
        {
            return View();

        }
    }
}
