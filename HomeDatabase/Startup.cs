using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.Routing;

namespace HomeDatabase
{
    public class Startup
    {
        SqlConnect connect;
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            
            SqlConnect.conStr = ConfigurationExtensions.GetConnectionString(this.Configuration, "Home");
            connect = new SqlConnect();
            connect.OpenCon();
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.AddSingleton<IConfiguration>(Configuration);

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
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Acount}/{action=AcountView}/{id?}");
            });
        }




    }
}
