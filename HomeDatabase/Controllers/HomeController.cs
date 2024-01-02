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


        [HttpGet("/Home/HandleError")]
        public IActionResult HandleError(int? statusCode = null)
        {
            if (statusCode.HasValue && statusCode == 404)
            {
                return View("NotFound");
            }
            // You can add additional cases for other status codes if needed
            // For example, you can create custom views for 500 Internal Server Errors, etc.

            return View("Error"); // A fallback error view for other status codes.
        }

    }
}
