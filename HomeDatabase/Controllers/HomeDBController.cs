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
            HttpContext.Session.SetString("PreviousUrl", HttpContext.Request.Headers["Referer"].ToString());

            List<TableViewModel> tables = SqlConnect.Instance.GetTables();
            return View(tables);
        }
        public IActionResult GoBack()
        {
            string previousUrl = HttpContext.Session.GetString("PreviousUrl");
            if (previousUrl != null)
            {
                return Redirect(previousUrl);
            }
            return RedirectToAction("Index", "HomeDB");
        }

        public IActionResult Create()
        {
            //Create Table on Home DB
            return View();
        }

        public IActionResult Delete()
        {
            //Drop Table on Home DB
            return View();
        }





    }
}