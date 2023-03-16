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

        public IActionResult Database()
        {
            SqlConnect loaddata = new SqlConnect();
            loaddata.retrieveData("Select * From Servers");

            List<Databases> databases = loaddata.GetDatabaseList();

            return View(databases);
        }

    }
}
