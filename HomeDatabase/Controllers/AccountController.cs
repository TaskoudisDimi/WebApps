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
using Microsoft.Extensions.Options;
using HomeDatabase.Helpers;
using Microsoft.AspNetCore.DataProtection.KeyManagement;

namespace HomeDatabase.Controllers
{
    public class AccountController : Controller
    {

        public readonly Authentication_Service authService;

        public AccountController(Authentication_Service authService)
        {
            this.authService = authService;
        }

        #region Login
        
        [HttpGet]
        public async Task<IActionResult> LogIn()
        {
            var schemes = await HttpContext.RequestServices.GetRequiredService<IAuthenticationSchemeProvider>().GetAllSchemesAsync();
            var model = new UsersViewModel
            {
                ExternalLogins = schemes.Where(x => !string.IsNullOrEmpty(x.DisplayName)).ToList()
            };
            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> LogIn(UsersViewModel user)
        {
            var user_ = authService.ValidateUser(user);
            if (!user.PendingRegistration.Equals(true))
            {
                if (user_ != null)
                {
                    bool isAdmin = user_.isAdmin == true ? true : false;
                    var roles = isAdmin ? new[] { "Admin" } : new[] { "User" };
                    var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.NameIdentifier, user_.Id.ToString()),
                };
                    // Add role claims
                    claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));
                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var principal = new ClaimsPrincipal(identity);
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                    // Assign roles to the user
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity), new AuthenticationProperties
                    {
                        IsPersistent = true,
                        ExpiresUtc = DateTime.UtcNow.AddHours(1) // Set the expiration time as needed
                    });
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    // Handle login failure
                    ModelState.AddModelError("LoginError", "Invalid username or password.");
                    return RedirectToAction("Error", "Account", new { loginError = "Invalid username or password." });
                }
            }
            else
            {
                // Handle login failure
                ModelState.AddModelError("LoginError", "Something went wrong");
                return RedirectToAction("Error", "Account", new { loginError = "Something went wrong." });
            }

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


        #endregion


        #region Register

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }


        [HttpPost("RegisterUser")]
        public IActionResult RegisterUser(UsersViewModel model)
        {
            string token = authService.RegisterAccount(model);
            if (token == null)
            {
                // Handle registration failure
                // Handle login failure
                ModelState.AddModelError("RegistrationError", "Registration failed.");
                return RedirectToAction("Error", "Account", new { registrationError = "Registration failed." });
            }
            return RedirectToAction("EmailConfirmed", "Account");
        }

        public IActionResult EmailConfirmed()
        {
            return View();
        }

        [HttpGet("VerifyEmail")]
        public IActionResult VerifyEmail([FromQuery] string token)
        {
            bool isEmailVerified = authService.VerifyEmail(token);
            if (isEmailVerified)
            {
                return RedirectToAction("Login", "Account");
            }
            ModelState.AddModelError("RegistrationError", "Email verification failed.");
            return RedirectToAction("Error", "Account", new { registrationError = "Email verification failed" });
        }

        #endregion

        public IActionResult Error(string loginError)
        {
            // Optionally, you can do something with the loginError parameter
            ViewData["Error"] = Error;
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("LogIn");
        }

        public IActionResult GoBack()
        {
            //string previousUrl = HttpContext.Session.GetString("PreviousUrl");
            //if (previousUrl != null)
            //{
            //    return Redirect(previousUrl);
            //}
            return RedirectToAction("Index", "Home");
        }


        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }


    }


}
