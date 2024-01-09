using HomeDatabase.Database;
using HomeDatabase.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace HomeDatabase.Controllers
{
    public class TODOController : Controller
    {
        public IActionResult Index()
        {
            DataTable servers = SqlConnect.Instance.SelectDataTable("SELECT ID, Name, Title, Description, DateCreated, DeliveryDate, Done FROM TODO");
            List<TODOViewModel> list = new List<TODOViewModel>();
            foreach (DataRow row in servers.Rows)
            {
                TODOViewModel passwordViewModel = new TODOViewModel
                {
                    ID = Convert.ToInt32(row["ID"]),
                    Name = row["Name"].ToString(),
                    Title = row["Title"].ToString(),
                    Description = row["Description"].ToString(),
                    DateCreated = Convert.ToDateTime(row["DateCreated"].ToString()),
                    DeliveryDate = Convert.ToDateTime(row["DeliveryDate"].ToString()),
                    Done = Convert.ToBoolean(row["Done"].ToString())
                };
                list.Add(passwordViewModel);
            }
            return View(list);
        }





    }
}
