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


        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(TODOViewModel todo)
        {

            if (SqlConnect.Instance.Insert($"Insert Into TODO Values ('{todo.Name}', '{todo.Title}'" +
                $", '{todo.Description}', '{todo.DateCreated}', '{todo.DeliveryDate}', '{todo.Done}')") > 0)
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
            DataTable table = SqlConnect.Instance.SelectDataTable($"SELECT ID, Name, Title, Description, DateCreated, DeliveryDate, Done From TODO where ID = '{id}'");
            TODOViewModel todo = new TODOViewModel();
            if (table.Rows.Count > 0)
            {
                foreach (DataRow row in table.Rows)
                {
                    todo.ID = Convert.ToInt32(row["ID"]);
                    todo.Name = row["Name"].ToString();
                    todo.Title = row["Title"].ToString();
                    todo.Description = row["Description"].ToString();
                    todo.DateCreated = Convert.ToDateTime(row["DateCreated"].ToString());
                    todo.DeliveryDate = Convert.ToDateTime(row["DeliveryDate"].ToString());
                    todo.Done = Convert.ToBoolean(row["Done"].ToString());
                }
                return View(todo);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost]
        public IActionResult Edit(TODOViewModel todo)
        {

            if (SqlConnect.Instance.Update($"Update TODO set Name = '{todo.Name}', " +
                $"Description = '{todo.Description}', Title = '{todo.Title}', " +
                $"DateCreated = '{todo.DateCreated}', DeliveryDate = '{todo.DeliveryDate}', Done = '{todo.Done}' where ID = {todo.ID}") > 0)
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
            DataTable table = SqlConnect.Instance.SelectDataTable($"Select * From TODO where ID = '{id}'");
            TODOViewModel todo = new TODOViewModel();
            if (table.Rows.Count > 0)
            {
                foreach (DataRow row in table.Rows)
                {
                    todo.ID = Convert.ToInt32(row["ID"]);
                    todo.Name = row["Name"].ToString();
                    todo.Title = row["Title"].ToString();
                    todo.Description = row["Description"].ToString();
                    todo.DateCreated = Convert.ToDateTime(row["DateCreated"].ToString());
                    todo.DeliveryDate = Convert.ToDateTime(row["DeliveryDate"].ToString());
                    todo.Done = Convert.ToBoolean(row["Done"].ToString());
                }
                return View(todo);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost]
        public IActionResult Delete(TODOViewModel todo)
        {
            if (SqlConnect.Instance.Delete($"Delete from TODO where ID = {todo.ID}") > 0)
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
            return RedirectToAction("Index", "Keys");
        }




    }
}
