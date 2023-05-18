using Dog.Models;
using Microsoft.AspNetCore.Mvc;

namespace Dog.Controllers
{
    public class CategoryController : Controller
    {
        private static List<CatViewModel> categories = new List<CatViewModel>();

        public IActionResult Index()
        {
            return View(categories);
        }

        public IActionResult Create()
        {
            var cat = new CatViewModel();
            return View(cat);
        }

        public IActionResult CreateCat(CatViewModel cat)
        {
            categories.Add(cat);
            return RedirectToAction(nameof(Index));
            //return View("Index");
        }


    }
}
