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
            sqlConnect.execCom($"Insert Into Servers Values ('{servers.Name}')");
            return RedirectToAction("Index");
        }

        public IActionResult Edit(int? id)
        {
            return View();
        }

        [HttpPost]
        public IActionResult Edit(ServersViewModel servers)
        {
            SqlConnect sqlConnect = new SqlConnect();
            sqlConnect.execCom($"Update Servers set Name = '{servers.Name}' where Id = {servers.Id}");
            return RedirectToAction("Index");
        }

        public IActionResult Delete()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            SqlConnect sqlConnect = new SqlConnect();
            sqlConnect.execCom($"Delete from Servers where Id = {id}");
            return View($"Deleted {id}");
        }


    }
}
