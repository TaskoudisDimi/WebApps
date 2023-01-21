using Books.Data;
using Books.Models;
using Microsoft.AspNetCore.Mvc;

namespace Books.Controllers
{
    public class CategoryController : Controller
    {

        private readonly ApplicationDbContext _db;
        
        public CategoryController(ApplicationDbContext db)
        {
            //This db will have all the implementation of connection strings, tables in order to retrieve data.
            _db = db;
        }

        public IActionResult Index()
        {
            IEnumerable<Category> objCategoryList = _db.Categories;
            return View(objCategoryList);
        }
    }
}
