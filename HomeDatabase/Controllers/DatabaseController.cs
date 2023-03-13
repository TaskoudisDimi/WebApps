using HomeDatabase.Models;
using Microsoft.AspNetCore.Mvc;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;


namespace HomeDatabase.Controllers
{
    public class DatabaseController : Controller
    {
        List<Servers> servers = new List<Servers>();
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Database()
        {
            SqlConnect loaddata = new SqlConnect();
            loaddata.retrieveData("Select * From Servers");
            DataTable test = loaddata.table;
            getData(test);
            return View(servers);
        }

        private List<Servers> getData(DataTable data)
        {
            servers = data.AsEnumerable()
                .Select(row => new Servers
                {
                    Id = row.Field<int>("Id"),
                    Name = row.Field<string>("Name")
                }).ToList();
            return servers;
        }

       

    }
}
