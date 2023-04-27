using HomeDatabase.Models;
using Microsoft.AspNetCore.Mvc;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;

namespace HomeDatabase.Controllers
{
    public class DatabaseController : Controller
    {   
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult HomeDatabases()
        {
            SqlConnect loaddata = new SqlConnect();
            List<Databases> databases = loaddata.GetDatabaseList();
            return View(databases);
        }

        public IActionResult Table()
        {
            return View("TableDatabase");
        }
    }
}
