using HomeDatabase.Models;
using Microsoft.AspNetCore.Mvc;

namespace HomeDatabase.Controllers
{
    public class ServersController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public string Error()
        {
            return "Error";
        }


        [HttpPost]
        public IActionResult create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult create(Servers servers)
        {
            SqlConnect sqlConnect = new SqlConnect();
            sqlConnect.execCom($"Insert Into Servers Values ('{servers.Name}')");
            return View(servers);
        }

    }
}
