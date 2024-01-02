using HomeDatabase.Database;
using HomeDatabase.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace HomeDatabase.Controllers
{
    public class UsersController : Controller
    {
        public IActionResult Index()
        {

            HttpContext.Session.SetString("PreviousUrl", HttpContext.Request.Headers["Referer"].ToString());

            DataTable users = SqlConnect.Instance.SelectDataTable("Select * From Users");
            List<UsersViewModel> list = users.AsEnumerable()
                                .Select(row => new UsersViewModel
                                {
                                    Id = Convert.ToInt32(row["Id"]),
                                    Username = row["Username"].ToString()
                                })
                                .ToList();

            return View(list);
        }


        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(UsersViewModel user)
        {
            if (SqlConnect.Instance.Insert($"Insert Into Users Values ('{user.Username}')") > 0)
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
            DataTable table = SqlConnect.Instance.SelectDataTable($"Select * From Users where Id = '{id}'");
            UsersViewModel user = new UsersViewModel();
            if (table.Rows.Count > 0)
            {
                foreach (DataRow row in table.Rows)
                {
                    user.Id = Convert.ToInt32(row["Id"]);
                    user.Username = row["Username"].ToString();
                    user.Password = row["Password"].ToString();
                    user.isAdmin = (bool)row["isAdmin"];

                }
                return View(user);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost]
        public IActionResult Edit(UsersViewModel user)
        {

            if (SqlConnect.Instance.Update($"Update Users set Name = '{user.Username}' where Id = {user.Id}") > 0)
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
            DataTable table = SqlConnect.Instance.SelectDataTable($"Select * From Users where Id = '{id}'");
            UsersViewModel user = new UsersViewModel();
            if (table.Rows.Count > 0)
            {
                foreach (DataRow row in table.Rows)
                {
                    user.Id = Convert.ToInt32(row["Id"]);
                    user.Username = row["Username"].ToString();
                }
                return View(user);
            }
            else
            {
                return NotFound();
            }

        }

        [HttpPost]
        public IActionResult Delete(UsersViewModel user)
        {
            if (SqlConnect.Instance.Delete($"Delete from Users where Id = {user.Id}") > 0)
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
