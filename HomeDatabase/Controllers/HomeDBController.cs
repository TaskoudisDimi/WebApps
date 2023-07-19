using Azure;
using HomeDatabase.Database;
using HomeDatabase.Models;
using Microsoft.AspNetCore.Hosting.Server;
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

        [HttpPost]
        public IActionResult Delete(string TableName)
        {
            List<TableViewModel> tableToDelete = SqlConnect.Instance.GetTables("Servers");
            TableViewModel table = tableToDelete.FirstOrDefault(t => t.TableName == TableName);
            if (table != null)
            {
                return View(table);
            }
            else
            {
                return NotFound();
            }
        }


        
        public IActionResult DeleteTable(string TableName)
        {
            if (SqlConnect.Instance.Delete($"Drop Table {TableName}") > 0)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return NotFound();
            }

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