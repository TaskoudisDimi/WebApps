using Microsoft.AspNetCore.Mvc;

namespace HomeDatabase.Controllers
{
    public class KeysController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
