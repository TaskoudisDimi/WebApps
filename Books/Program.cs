using Books.Data;
using Microsoft.EntityFrameworkCore;
using Books.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// This is different on Web App with Razor Pages
builder.Services.AddControllersWithViews();
//container contain dependency injection

builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(
    builder.Configuration.GetConnectionString("Book")));

var app = builder.Build();

// Configure the HTTP request pipeline.
// This pipeline display how to my web app should respond to web request
// The web request which I get from browser, goes back and forth through this pipeline
if (!app.Environment.IsDevelopment())
{

    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
//UseExceptionHandler,UseHttpsRedirection,UseStaticFiles ---> Is Middleware
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

//If I want to use authentication, need define Middleware
//app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
//Shared View -> partial views. We can call within a view in multiple places in my app.