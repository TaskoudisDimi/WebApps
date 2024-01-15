using HomeDatabase.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Mail;
using System.Data;
using Microsoft.Data.SqlClient;
using HomeDatabase.Database;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using System.Text;
using System;
using Microsoft.AspNetCore.Authentication;


namespace HomeDatabase.Controllers
{
    public class AccountController : Controller
    {

        public IActionResult AccountView()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> LogIn()
        {
            var schemes = await HttpContext.RequestServices.GetRequiredService<IAuthenticationSchemeProvider>().GetAllSchemesAsync();

            var model = new LogInViewModel
            {
                ExternalLogins = schemes.Where(x => !string.IsNullOrEmpty(x.DisplayName)).ToList()
            };

            return View(model);
        }

        [HttpGet]
        public IActionResult ExternalLogin(string provider)
        {
            // Request a redirect to the external login provider
            var redirectUrl = Url.Action(nameof(ExternalLoginCallback), "Account");
            var properties = new AuthenticationProperties { RedirectUri = redirectUrl };

            return Challenge(properties, provider);
        }

        [HttpGet]
        public async Task<IActionResult> ExternalLoginCallback()
        {
            var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            if (result?.Succeeded != true)
            {
                // Handle the error, redirect, or display an error view
                return RedirectToAction("LogIn");
            }

            // External login successful, retrieve user information
            var externalClaims = result.Principal?.Claims;
            var claims = new List<Claim>();

            if (externalClaims != null)
            {
                claims.AddRange(externalClaims);
            }

            // Add additional claims or process the external user information as needed

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> LogIn(LogInViewModel login)
        {
            DataTable table = SqlConnect.Instance.SelectDataTable($"Select * From Users where Username = '{login.Username}' And Password = '{login.Password}'");
            if (table.Rows.Count > 0)
            {
                var claims = new[]
                {
                    new Claim(ClaimTypes.Name, login.Username)
                };
                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));
                return RedirectToAction("Index", "Home");
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
        public async Task<IActionResult> Register(LogInViewModel model)
        {
            // Generate a verification token (e.g., a GUID)
            // Generate Token
            string verificationToken = Guid.NewGuid().ToString();

            // Save the verification token and user details to the database
            //Check if the user is in the database
            DataTable table = SqlConnect.Instance.SelectDataTable($"Select * From Users where Username = '{model.Username}' And Password = '{model.Password}'");
            if (table.Rows.Count > 0)
            {
                return View("LogIn");
            }
            else
            {
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

                
        }

        public IActionResult EmailConfirmed()
        {
            return View();
        }


        // Action method to handle account verification
        public IActionResult VerifyAccount(string token)
        {
            LogInViewModel user = new LogInViewModel();
            DataTable table = SqlConnect.Instance.SelectDataTable($"Select * From Users where Token = '{token}'");
            if (table.Rows.Count > 0)
            {
                return RedirectToAction("ListOfDatabases", "Databases");
            }
            else
            {
                return View("Register");
            }
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("LogIn");
        }

        public IActionResult GoBack()
        {
            string previousUrl = HttpContext.Session.GetString("PreviousUrl");
            if (previousUrl != null)
            {
                return Redirect(previousUrl);
            }
            return RedirectToAction("Index", "Account");
        }

    }


}
