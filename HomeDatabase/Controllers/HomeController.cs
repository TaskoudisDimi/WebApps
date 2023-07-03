using HomeDatabase.Database;
using Microsoft.AspNetCore.Mvc;

namespace HomeDatabase.Controllers
{

   
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            HttpContext.Session.SetString("PreviousUrl", HttpContext.Request.Headers["Referer"].ToString());

            return View();
        }

        public IActionResult GoBack()
        {
            string previousUrl = HttpContext.Session.GetString("PreviousUrl");
            if (previousUrl != null)
            {
                return Redirect(previousUrl);
            }
            return RedirectToAction("Index", "Home");
        }
        public IActionResult Unauthorized()
        {
            return View();
        }

    }
}
