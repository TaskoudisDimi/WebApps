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

       
        public IActionResult Create(ServersViewModel servers)
        {
            SqlConnect sqlConnect = new SqlConnect();
            sqlConnect.execCom($"Insert Into Servers Values ('{servers.Name}')");
            return View(servers);
        }


        public IActionResult Edit(ServersViewModel servers)
        {
            SqlConnect sqlConnect = new SqlConnect();
            sqlConnect.execCom($"Edit Values ('{servers.Name}')");
            return View(servers);
        }


        public IActionResult Delete(int id)
        {
            SqlConnect sqlConnect = new SqlConnect();
            sqlConnect.execCom($"Delete from Servers where Id = {id}");
            return View($"Deleted {id}");
        }


    }
}
