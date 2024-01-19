using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace HomeDatabase
{
    public class Program
    {
        //TODO: Create Mechanism in order to caching data
        //TODO: Error message on Login/Register form
        //TODO: Google Calendar
        //TODO: Database Backup, Restore, Delete, Clear, Add/Edit/Delete/Update Table, Add Database
        //TODO: Database Trancate
        //TODO: Docker

        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
