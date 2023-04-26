using Microsoft.AspNetCore.Mvc;

namespace HomeDatabase.Controllers
{
    public class TableController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
