using HomeDatabase.Database;
using HomeDatabase.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Reflection;

namespace HomeDatabase.Controllers
{
    [Authorize(Roles = "Admin")]
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
                                    Username = row["Username"].ToString(),
                                    Password = row["Password"].ToString(),
                                    Email = row["Email"].ToString(),
                                    isAdmin = Convert.ToBoolean(row["isAdmin"]),
                                    PendingRegistration = Convert.ToBoolean(row["PendingRegistration"])
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
            // Hash the password before saving to the database
            string passwordHash = Utils.HashPassword(user.Password);
            DataTable table = SqlConnect.Instance.SelectDataTable($"Select * From Users where Username = '{user.Username}'");
            if (!(table.Rows.Count > 0))
            {
                SqlConnect.Instance.ExecuteNQ($"Insert Into Users Values ('{user.Username}', '{passwordHash}'" +
                $", '{user.Email}', '{user.Token}','{user.isAdmin}','{user.PendingRegistration}', '{user.resetPassword}')");
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
                    user.Email = row["Email"].ToString();
                    user.isAdmin = (bool)row["isAdmin"];
                    user.PendingRegistration = (bool)row["PendingRegistration"];
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

            if (SqlConnect.Instance.ExecuteNQ($"Update Users set Username = '{user.Username}', Password = '{user.Username}'" +
                $", Email = '{user.Email}', isAdmin = '{user.isAdmin}', PendingRegistration = '{user.PendingRegistration}'" +
                $" where Id = {user.Id}") > 0)
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
            if (SqlConnect.Instance.ExecuteNQ($"Delete from Users where Id = {user.Id}") > 0)
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
            return RedirectToAction("Index", "Home");
        }


    }
}
