using HomeDatabase.Database;
using HomeDatabase.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Diagnostics;

namespace HomeDatabase.Controllers
{
    public class HomeDBController : Controller
    {

        public IActionResult Index()
        {
            List<TableViewModel> tables = SqlConnect.Instance.GetTables();
            return View(tables);

        }

    }
}