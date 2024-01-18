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
            DataTable servers = SqlConnect.Instance.SelectDataTable("SELECT ID, FirstName, LastName, Username, Password, Service, encryptionKey FROM Passwords");

            List<PasswordsViewModel> list = new List<PasswordsViewModel>();

            foreach (DataRow row in servers.Rows)
            {
                string encryptionKey = row["encryptionKey"].ToString();
                string encryptedPassword = row["Password"].ToString();

                // Decrypt password using encryption key for this row
                string decryptedPassword = Utils.DecryptPassword(encryptedPassword, encryptionKey);

                PasswordsViewModel passwordViewModel = new PasswordsViewModel
                {
                    Id = Convert.ToInt32(row["ID"]),
                    FirstName = row["FirstName"].ToString(),
                    LastName = row["LastName"].ToString(),
                    Username = row["Username"].ToString(),
                    Password = decryptedPassword,
                    Service = row["Service"].ToString(),
                    encryptionKey = encryptionKey // Optional: Assign encryption key to view model
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
        public IActionResult Create(PasswordsViewModel key)
        {
            string encryptionKey = Utils.GenerateEncryptionKey(256);

            if (SqlConnect.Instance.ExecuteNQ($"Insert Into Passwords Values ('{key.FirstName}', '{key.LastName}'" +
                $", '{key.Username}', '{Utils.EncryptPassword(key.Password, encryptionKey)}', '{key.Service}', '{encryptionKey}')") > 0)
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
            DataTable table = SqlConnect.Instance.SelectDataTable($"SELECT ID, FirstName, LastName, Username, Password, Service From Passwords where ID = '{id}'");
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
            DataTable table = SqlConnect.Instance.SelectDataTable($"Select Password,encryptionKey From Passwords where ID = {key.Id}");
            PasswordsViewModel keyExisting = new PasswordsViewModel();
            if (table.Rows.Count > 0)
            {
                foreach (DataRow row in table.Rows)
                {
                    keyExisting.Password = row["Password"].ToString();
                    keyExisting.encryptionKey = row["encryptionKey"].ToString();
                }
            }

            string newPassword = key.Password;
            string encryptionKey;
            
            if (keyExisting.Password != null && !keyExisting.Password.Equals(newPassword))
            {
                // Password has changed; generate a new encryption key
                encryptionKey = Utils.GenerateEncryptionKey(256);
                key.Password = Utils.EncryptPassword(key.Password, encryptionKey);
            }
            else
            {
                // Password remains unchanged; use the existing encryption key
                encryptionKey = keyExisting.encryptionKey;
            }

            if (SqlConnect.Instance.ExecuteNQ($"Update Passwords set FirstName = '{key.FirstName}', " +
                $"LastName = '{key.LastName}', Username = '{key.Username}', " +
                $"Password = '{key.Password}', Service = '{key.Service}', encryptionKey = '{encryptionKey}' where ID = {key.Id}") > 0)
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
        public IActionResult Delete(PasswordsViewModel key)
        {
            if (SqlConnect.Instance.ExecuteNQ($"Delete from Passwords where Id = {key.Id}") > 0)
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
