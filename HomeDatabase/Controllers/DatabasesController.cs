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
            List<DatabasesModel> databases = SqlConnect.Instance.GetDatabaseList();
            return View(databases);
        }

        public IActionResult database(DatabasesModel database)
        {
            
            return RedirectToAction("Index", $"{database.Name}");
        }

        public IActionResult GoBack()
        {
           
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Create()
        {

            return View();
        }

        public IActionResult CreateDatabase(DatabasesModel database)
        {
            if (SqlConnect.Instance.Insert($"Insert Into Test") > 0)
            {
                return View();
            }
            else
            {
                return NotFound();
            }
        }

        public IActionResult Delete(DatabasesModel database)
        {
            return View(database);
        }


        public IActionResult DeleteDatabase(DatabasesModel Database)
        {
            if (SqlConnect.Instance.Delete($"Drop Database where database_id {Database.id}") > 0)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return NotFound();
            }

        }



    }
}
