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
            List<Databases> databases = SqlConnect.Instance.GetDatabaseList();
            return View(databases);
        }

        public IActionResult database(Databases database)
        {
            
            return RedirectToAction("Index", $"{database.Name}");
        }

        public IActionResult GoBack()
        {
           
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Create()
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

        public IActionResult Delete(ServersViewModel server)
        {
            if (SqlConnect.Instance.Delete($"Delete from Test where id = {server.Id}") > 0)
            {
                return View();
            }
            else
            {
                return NotFound();
            }
        }


    }
}
