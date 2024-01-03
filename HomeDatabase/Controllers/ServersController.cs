using HomeDatabase.Database;
using HomeDatabase.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace HomeDatabase.Controllers
{
    
    [Authorize]
    public class ServersController : Controller
    {
        public IActionResult Index()
        {

            HttpContext.Session.SetString("PreviousUrl", HttpContext.Request.Headers["Referer"].ToString());

            DataTable servers = SqlConnect.Instance.SelectDataTable("Select * From Servers");
            List<ServersViewModel> list = servers.AsEnumerable()
                                .Select(row => new ServersViewModel
                                {
                                    Id = Convert.ToInt32(row["Id"]),
                                    Name = row["Name"].ToString()
                                })
                                .ToList();
            
            return View(list);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(ServersViewModel servers)
        {
            if (SqlConnect.Instance.Insert($"Insert Into Servers Values ('{servers.Name}')") > 0)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return NotFound();
            }
            
        }

        public IActionResult Edit(int? id)
        {
            DataTable table = SqlConnect.Instance.SelectDataTable($"Select * From Servers where Id = '{id}'");
            ServersViewModel server = new ServersViewModel();
            if(table.Rows.Count > 0)
            {
                foreach (DataRow row in table.Rows)
                {
                    server.Id = Convert.ToInt32(row["Id"]);
                    server.Name = row["Name"].ToString();
                }
                return View(server);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost]
        public IActionResult Edit(ServersViewModel server)
        {

            if(SqlConnect.Instance.Update($"Update Servers set Name = '{server.Name}' where Id = {server.Id}") > 0)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return NotFound();
            }

        }

        public IActionResult Delete(int? id)
        {
            DataTable table = SqlConnect.Instance.SelectDataTable($"Select * From Servers where Id = '{id}'");
            ServersViewModel server = new ServersViewModel();
            if (table.Rows.Count > 0)
            {
                foreach (DataRow row in table.Rows)
                {
                    server.Id = Convert.ToInt32(row["Id"]);
                    server.Name = row["Name"].ToString();
                }
                return View(server);
            }
            else
            {
                return NotFound();
            }
            
        }

        [HttpPost]
        public IActionResult Delete(ServersViewModel server)
        {
            if(SqlConnect.Instance.Delete($"Delete from Servers where Id = {server.Id}") > 0)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return NotFound();
            }
        }

        public IActionResult GoBack()
        {
            
            return RedirectToAction("Index", "Databases");
        }

    }
}
