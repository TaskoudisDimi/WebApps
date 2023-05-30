﻿using HomeDatabase.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Mail;
using System.Data;
using Microsoft.Data.SqlClient;

namespace HomeDatabase.Controllers
{
    public class AccountController : Controller
    {

        public IActionResult AccountView()
        {
            return View();
        }

        [HttpGet]
        public IActionResult LogIn()
        {
            return View();
        }

        [HttpPost]
        public IActionResult LogIn(LogInViewModel login)
        {
            SqlConnect loadUser = new SqlConnect();
            loadUser.retrieveData($"Select * From Users where Username = '{login.Username}' And Password = '{login.Password}'");
            if (loadUser.table.Rows.Count > 0)
            {
                return RedirectToAction("ListOfDatabases", "Databases");
            }
            else
            {
                return View("LogIn");
            }
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            // Generate a verification token (e.g., a GUID)
            string verificationToken = Guid.NewGuid().ToString();

            // Save the verification token and user details to the database
            //Check if the user is in the database
            SqlConnect loadUser = new SqlConnect();
            loadUser.retrieveData($"Select * From Users where token = {verificationToken}");
            if (!(loadUser.table.Rows.Count > 0))
            {
                loadUser.execNonQuery($"Insert Into Users values ('Test','1234','{verificationToken}')");
            }
            // Send verification email
            string callbackUrl = Url.Action("VerifyAccount", "Account", new { token = verificationToken }, protocol: HttpContext.Request.Scheme);

            MailMessage message = new MailMessage();
            message.From = new MailAddress(model.Email);
            message.To.Add(new MailAddress(model.Email));
            message.Subject = "Account Verification";
            message.Body = $"Dear {model.Username},<br/><br/>Please click the link below to verify your account:<br/><br/><a href=\"{callbackUrl}\">{callbackUrl}</a>";
            message.IsBodyHtml = true;

            using (SmtpClient smtpClient = new SmtpClient("smtp-relay.sendinblue.com", 587))
            {
                smtpClient.Credentials = new NetworkCredential("taskoudisdimitris@gmail.com", "NZ0cqP31rFRT9gJL");
                smtpClient.EnableSsl = true;
                await smtpClient.SendMailAsync(message);
            }

            return RedirectToAction("EmailConfirmed");
        }

        public IActionResult EmailConfirmed()
        {
            return View();
        }


        // Action method to handle account verification
        public IActionResult VerifyAccount(string token)
        {
            LogInViewModel user = new LogInViewModel();
            SqlConnect loadUser = new SqlConnect();
            loadUser.retrieveData($"Select * From Users where Token = '{token}'");
            if (loadUser.table.Rows.Count > 0)
            {
                return RedirectToAction("ListOfDatabases", "Databases");
            }
            else
            {
                return View("Register");
            }

        }

    }


}
