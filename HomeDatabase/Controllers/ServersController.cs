using HomeDatabase.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace HomeDatabase.Controllers
{
    public class ServersController : Controller
    {
        public IActionResult Index()
        {
            SqlConnect loaddata = new SqlConnect();
            loaddata.retrieveData("Select * From Servers");
            DataTable servers = loaddata.table;
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
            SqlConnect sqlConnect = new SqlConnect();
            sqlConnect.execNonQuery($"Insert Into Servers Values ('{servers.Name}')");
            return RedirectToAction("Index");
        }

        public IActionResult Edit(int? id)
        {
            SqlConnect sql = new SqlConnect();
            sql.retrieveData($"Select * From Servers where Id = '{id}'");
            DataTable table = sql.table;
            ServersViewModel server = new ServersViewModel();
            
            foreach(DataRow row in table.Rows)
            {
                server.Id = Convert.ToInt32(row["Id"]);
                server.Name = row["Name"].ToString(); 
            }
            return View(server);
        }

        [HttpPost]
        public IActionResult Edit(ServersViewModel server)
        {
            SqlConnect sqlConnect = new SqlConnect();
            sqlConnect.execNonQuery($"Update Servers set Name = '{server.Name}' where Id = {server.Id}");
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int? id)
        {
            SqlConnect sql = new SqlConnect();
            sql.retrieveData($"Select * From Servers where Id = '{id}'");
            DataTable table = sql.table;
            ServersViewModel server = new ServersViewModel();

            foreach (DataRow row in table.Rows)
            {
                server.Id = Convert.ToInt32(row["Id"]);
                server.Name = row["Name"].ToString();
            }
            return View(server);
        }

        [HttpPost]
        public IActionResult Delete(ServersViewModel server)
        {
            SqlConnect sqlConnect = new SqlConnect();
            sqlConnect.execNonQuery($"Delete from Servers where Id = {server.Id}");
            return RedirectToAction("Index");
        }


    }
}
