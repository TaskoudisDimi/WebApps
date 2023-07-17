using Azure;
using HomeDatabase.Database;
using HomeDatabase.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Diagnostics;
using System.Net.Sockets;

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

        public IActionResult Create(List<TableViewModel> tableData)
        {
            //Create Table on Home DB
            return View();
        }

        public IActionResult Delete()
        {
            //Drop Table on Home DB
            return View();
        }
        
        public IActionResult CleanDB()
        {
            if (SqlConnect.Instance.CleanDB())
                return RedirectToAction("Success", "HomeDB");
            else
                return RedirectToAction("Error", "HomeDB");
        }

        public IActionResult Error()
        {
            return View();
        }


        public string RestoreDB()
        {
            return "success";
        }


        public IActionResult BackupDB()
        {
            if (SqlConnect.Instance.Backup("HomeDB"))
                return RedirectToAction("Success", "HomeDB");
            else
                return RedirectToAction("Error", "HomeDB");
        }


    }
}