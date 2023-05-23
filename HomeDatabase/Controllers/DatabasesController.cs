using HomeDatabase.Models;
using Microsoft.AspNetCore.Mvc;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;

namespace HomeDatabase.Controllers
{
    public class DatabasesController : Controller
    {   

        public IActionResult ListOfDatabases()
        {
            SqlConnect loaddata = new SqlConnect();
            List<Databases> databases = loaddata.GetDatabaseList();
            return View(databases);
        }

        public IActionResult Tables(Databases table)
        {

            return RedirectToAction("Index", $"{table.Name}");
            
        }
    }
}
