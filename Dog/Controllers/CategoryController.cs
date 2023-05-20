using Dog.Data;
using Dog.Models;
using Microsoft.AspNetCore.Mvc;

namespace Dog.Controllers
{
    public class CategoryController : Controller
    {
        private static List<CatViewModel> categories = new List<CatViewModel>();
        private readonly CategoryDbContext dbContext;
        
        public CategoryController(CategoryDbContext db)
        {
            dbContext = db;
        }

        public IActionResult Index()
        {
            IEnumerable<CatViewModel> categoriesDb = dbContext.Categories;
            return View(categoriesDb);
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
