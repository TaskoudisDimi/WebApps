using HomeDatabase.Database;
using HomeDatabase.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Diagnostics;

namespace HomeDatabase.Controllers
{
    public class HomeController : Controller
    {
       
        public IActionResult Index()
        {
            SqlConnect loaddata = new SqlConnect();
            List<TableViewModel> databases = loaddata.GetTables();
            return View(databases);

        }

       
    }
}