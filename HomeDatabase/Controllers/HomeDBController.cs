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
            if (SqlConnect.Instance.ExecuteNQ($"Drop Table {TableName}") > 0)
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


        [HttpPost]
        public async Task<IActionResult> RestoreDB(IFormFile backupFile)
        {
            if (backupFile != null && backupFile.Length > 0)
            {
                try
                {
                    // Define a path to save the uploaded file temporarily
                    var tempFilePath = Path.GetTempFileName();

                    // Copy the uploaded file to the temporary path
                    using (var stream = new FileStream(tempFilePath, FileMode.Create))
                    {
                        await backupFile.CopyToAsync(stream);
                    }

                    // Use the tempFilePath in your SQLConnect to perform the restore
                    if (SqlConnect.Instance.RestoreDB(tempFilePath, "HomeDB"))
                    {
                        // Perform operations after successful restore
                        return RedirectToAction("Success", "HomeDB");
                    }
                }
                catch (Exception ex)
                {
                    // Handle exceptions or errors, log them, etc.
                    return RedirectToAction("Error", "HomeDB");
                }
            }

            // Handle cases where no file was uploaded or an error occurred
            return RedirectToAction("Error", "HomeDB");
        }

        public IActionResult Success()
        {
            return View();
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