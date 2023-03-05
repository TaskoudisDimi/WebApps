using Microsoft.AspNetCore.Mvc;
using System.Configuration;
using System.Data.SqlClient;
using System.Drawing;


namespace HomeDatabase.Controllers
{
    public class DatabaseController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Database()
        {
            return View();
        }

        private readonly IConfiguration configuration;

        public DatabaseController(IConfiguration config)
        {
            configuration = config;
        }



    }
}
