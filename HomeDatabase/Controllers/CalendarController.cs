using Microsoft.AspNetCore.Mvc;

namespace HomeDatabase.Controllers
{
    public class CalendarController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
