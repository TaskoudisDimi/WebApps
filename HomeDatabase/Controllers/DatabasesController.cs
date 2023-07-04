using HomeDatabase.Database;
using HomeDatabase.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HomeDatabase.Controllers
{
    public class DatabasesController : Controller
    {

        [Authorize]
        public IActionResult ListOfDatabases()
        {

            SqlConnect loaddata = new SqlConnect();
            List<Databases> databases = SqlConnect.Instance.GetDatabaseList();
            return View(databases);
        }

        public IActionResult Tables(Databases table)
        {
            return RedirectToAction("Index", $"{table.Name}");
            
        }

        public IActionResult GoBack()
        {
           
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Create()
        {
            return View();
        }

        public IActionResult Delete()
        {
            return View();
        }


    }
}
