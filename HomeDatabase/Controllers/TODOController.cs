using FirebaseAdmin.Messaging;
using HomeDatabase.Database;
using HomeDatabase.Helpers;
using HomeDatabase.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Globalization;
using System.Security.Claims;
using System.Threading.Tasks;

namespace HomeDatabase.Controllers
{
    [Authorize]
    public class TODOController : Controller
    {
        private readonly Helpers.WebSocket_Manager _webSocketManager;

        //private readonly NotificationService _notificationService;
        private static readonly Dictionary<string, string> UserDeviceTokens = new Dictionary<string, string>();

        public TODOController(Helpers.WebSocket_Manager webSocketManager)
        {
            _webSocketManager = webSocketManager;
        }


        public IActionResult Index()
        {
            // Get the ID of the currently authenticated user
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            DataTable servers = SqlConnect.Instance.SelectDataTable("SELECT ID, Name, Title, Description, DateCreated, DeliveryDate, Done FROM TODO " +
                                                                    $"where userID = '{userId}'");
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
        public async Task<IActionResult> Create(TODOViewModel todo)
        {
            string dateCreated = todo.DateCreated.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
            string deliveryDate = todo.DeliveryDate.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (SqlConnect.Instance.ExecuteNQ($"Insert Into TODO Values ('{todo.Name}', '{todo.Title}'" +
                $", '{todo.Description}', '{dateCreated}', '{deliveryDate}', '{todo.Done}', '{userId}')") > 0)
            {
                await _webSocketManager.SendNotificationAsync($"{userId}", "New TODO item created!");
                return RedirectToAction("Index");
            }
            else
            {
                return NotFound();
            }
        }

        //Android
        private string GetUserDeviceToken(string userName)
        {
            // Retrieve the device token for the user from the dictionary
            if (UserDeviceTokens.TryGetValue(userName, out var deviceToken))
            {
                return deviceToken;
            }

            return null;
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

            string dateCreated = todo.DateCreated.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
            string deliveryDate = todo.DeliveryDate.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);


            if (SqlConnect.Instance.ExecuteNQ($"Update TODO set Name = '{todo.Name}', " +
                $"Description = '{todo.Description}', Title = '{todo.Title}', " +
                $"DateCreated = '{dateCreated}', DeliveryDate = '{deliveryDate}', Done = '{todo.Done}' where ID = {todo.ID}") > 0)
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
            if (SqlConnect.Instance.ExecuteNQ($"Delete from TODO where ID = {todo.ID}") > 0)
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
