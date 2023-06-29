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
            List<TableViewModel> databases = SqlConnect.Instance.GetTables();
            return View(databases);

        }

       
    }
}