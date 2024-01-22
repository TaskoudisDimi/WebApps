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
            services.AddSingleton<NotificationService>();
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
            services.AddScoped<Authentication_Service>();
            services.AddScoped<GenerateToken>();
            services.AddScoped<EmailService>();
            services.AddDistributedMemoryCache();
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
