using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;

namespace ShoppingCartService
{
    public static class Program
    {
        public static void Main(string[] args) =>
            CreateHostBuilder(args).Build().Run();

        private static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
           .ConfigureWebHostDefaults(webBuilder =>
           {
               webBuilder.UseStartup<Startup>();
           })
           .ConfigureServices(services =>
           {
               services.AddHostedService<ActivitySimulationWorker>();
           });
        }

       
    }
}
