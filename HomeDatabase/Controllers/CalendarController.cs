using HomeDatabase.Database;
using HomeDatabase.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Globalization;
using System.Security.Claims;

namespace HomeDatabase.Controllers
{
    [Authorize]
    public class CalendarController : Controller
    {

        public IActionResult Index()
        {
            // Get the ID of the currently authenticated user
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            DataTable servers = SqlConnect.Instance.SelectDataTable("SELECT ID, Title, Date FROM Calendar " +
                                                                    $"where userID = '{userId}'");
            List<CalendarViewModel> list = new List<CalendarViewModel>();
            foreach (DataRow row in servers.Rows)
            {
                CalendarViewModel events = new CalendarViewModel
                {
                    Id = Convert.ToInt32(row["Id"]),
                    Title = row["Title"].ToString(),
                    Date = Convert.ToDateTime(row["Date"].ToString())
                };
                list.Add(events);
            }
            return View(list);
        }


        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        public IActionResult CreateEvent(string title, DateTime time)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            string timeEvent = time.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
            if (SqlConnect.Instance.ExecuteNQ($"Insert Into Calendar Values ('{title}', '{timeEvent}', '{userId}')") > 0)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return NotFound();
            }

        }


        [HttpPost]
        public IActionResult updateEvent(int eventId, string title, DateTime time)
        {
            string timeEvent = time.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
            if (SqlConnect.Instance.ExecuteNQ($"Update Calendar set Title = '{title}', " +
                $"Date = '{timeEvent}' where ID = {eventId}") > 0)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost]
        public IActionResult DeleteEvent(int eventId)
        {
            if (SqlConnect.Instance.ExecuteNQ($"Delete from Calendar where ID = {eventId}") > 0)
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
