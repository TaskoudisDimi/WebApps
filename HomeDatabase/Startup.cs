using HomeDatabase.Controllers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Data.SqlClient;
using System.Data;
using HomeDatabase.Database;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using HomeDatabase.Helpers;

namespace HomeDatabase
{
    public class Startup
    {

        //Startup.cs is a class in an ASP.NET Core application that configures the application's request pipeline
        //and services. It's where you define how the application responds to HTTP requests, what services it uses,
        //and how it's configured. The ConfigureServices method is used for configuring services
        //and the Configure method is used for configuring the HTTP request pipeline. 

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            SqlConnect.connectionString = ConfigurationExtensions.GetConnectionString(this.Configuration, "Home");
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            
            services.AddControllersWithViews();
            services.AddSingleton<IConfiguration>(Configuration);
            // Registers a service as a singleton, meaning that there is only one instance
            // of the service for the entire application's lifetime.
            //services.AddSingleton<NotificationService>();
            //services.AddSingleton<Helpers.WebSocket_Manager>();

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddGoogle(options =>
            {
                options.ClientId = "538416839180-djsm3hq4l52uhsep9a304ac1ui3njq4i.apps.googleusercontent.com";
                options.ClientSecret = "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";
                options.CallbackPath = "/signin-google";
            })
            .AddCookie(options =>
            {
                options.Cookie.Name = "HomeDatabase.AuthCookie";
                options.LoginPath = "/Account/LogIn";
                options.AccessDeniedPath = "/Account/AccessDenied";
            });
            services.AddAuthorization(options =>
            {
                options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
            });

            //Registers a service as scoped, meaning that a new instance is created once per request.
            services.AddScoped<Authentication_Service>();
            services.AddScoped<GenerateToken>();
            services.AddScoped<EmailService>();
            //Registers a distributed in-memory cache. This is often used for caching data across multiple server instances in a distributed environment.
            services.AddDistributedMemoryCache();
            //This code configures the session services. It sets the session timeout to 60 minutes
            //and configures the session cookie to be marked as essential (which means that the application
            //might not function properly if the cookie is not present) and HTTP-only (making it inaccessible
            //to client-side scripts).
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(60);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            }); 

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStatusCodePagesWithReExecute("/Home/HandleError", "?statusCode={0}");

            app.UseStaticFiles();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseWebSockets();
            app.UseMiddleware<WebSocketMiddleware>(new Helpers.WebSocket_Manager());
            //This line adds the session middleware to the application pipeline. It enables the usage of session state in your application.
            app.UseSession();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}");
            });
        }

    }
}
