using HomeDatabase.Database;
using HomeDatabase.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace HomeDatabase.Controllers
{
    [Authorize]
    public class KeysController : Controller
    {
        public IActionResult Index()
        {
            HttpContext.Session.SetString("PreviousUrl", HttpContext.Request.Headers["Referer"].ToString());
            DataTable servers = SqlConnect.Instance.SelectDataTable("Select * From Passwords");
            List<PasswordsViewModel> list = servers.AsEnumerable()
                                .Select(row => new PasswordsViewModel
                                {
                                    Id = Convert.ToInt32(row["Id"]),
                                    FirstName = row["FirstName"].ToString(),
                                    LastName = row["LastName"].ToString(),
                                    Username = row["Username"].ToString(),
                                    Password = row["Password"].ToString(),
                                    Service = row["Service"].ToString()
                                })
                                .ToList();
            return View(list);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(PasswordsViewModel key)
        {
            if (SqlConnect.Instance.Insert($"Insert Into Passwords Values ('{key.FirstName}', '{key.LastName}'" +
                $", '{key.Username}', '{key.Password}', '{key.Service}')") > 0)
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
            DataTable table = SqlConnect.Instance.SelectDataTable($"Select * From Passwords where Id = '{id}'");
            PasswordsViewModel key = new PasswordsViewModel();
            if (table.Rows.Count > 0)
            {
                foreach (DataRow row in table.Rows)
                {
                    key.Id = Convert.ToInt32(row["ID"]);
                    key.FirstName = row["FirstName"].ToString();
                    key.LastName = row["LastName"].ToString();
                    key.Username = row["Username"].ToString();
                    key.Password = row["Password"].ToString();
                    key.Service = row["Service"].ToString();
                }
                return View(key);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost]
        public IActionResult Edit(PasswordsViewModel key)
        {

            if (SqlConnect.Instance.Update($"Update Passwords set FirstName = '{key.FirstName}', " +
                $"LastName = '{key.LastName}', Username = '{key.Username}', " +
                $"Password = '{key.Password}', Service = '{key.Service}' where ID = {key.Id}") > 0)
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
            DataTable table = SqlConnect.Instance.SelectDataTable($"Select * From Passwords where ID = '{id}'");
            PasswordsViewModel key = new PasswordsViewModel();
            if (table.Rows.Count > 0)
            {
                foreach (DataRow row in table.Rows)
                {
                    key.Id = Convert.ToInt32(row["ID"]);
                    key.FirstName = row["FirstName"].ToString();
                    key.LastName = row["LastName"].ToString();
                    key.Username = row["Username"].ToString();
                    key.Password = row["Password"].ToString();
                    key.Service = row["Service"].ToString();
                }
                return View(key);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost]
        public IActionResult Delete(PasswordsViewModel server)
        {
            if (SqlConnect.Instance.Delete($"Delete from Passwords where Id = {server.Id}") > 0)
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
