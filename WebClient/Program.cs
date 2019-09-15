using System;
using Serilog;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.DependencyInjection;

namespace WebClient
{
    public class Program
    {
        public static void Main(string[] args)
        {
            IWebHost host = BuildWebHost(args);

            ConfigLogger(host);
            MigrateDatabase(host);

            host.Run();
        }

        private static void ConfigLogger(IWebHost host)
        {
            IConfiguration configuration = host.Services.GetService<IConfiguration>();
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .CreateLogger();
        }

        private static void MigrateDatabase(IWebHost host)
        {
            try
            {
                using (IServiceScope scope = host.Services.CreateScope())
                {
                    IServiceProvider services = scope.ServiceProvider;
                    DbContext context = services.GetRequiredService<DbContext>();
                    context.Database.Migrate();
                }
            }
            catch (Exception e)
            {
                Log.Error(e.Message);
            }
        }

        #region Deploy ASP.NET Core to IIS
        public static IWebHost BuildWebHost(string[] args)
        {
            IWebHostBuilder webHost = WebHost.CreateDefaultBuilder(args);

            // SetingHeaderWebHost(webHost).ConfigureLogging((hostingContext, logging) =>
            webHost.ConfigureLogging((hostingContext, logging) =>
                {
                    logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
                    logging.AddConsole();
                    logging.AddDebug();
                    logging.AddEventSourceLogger();
                })
                .UseStartup<Startup>();
            // SetingFooterWebHost(webHost);

            return webHost.Build();
        }
        #endregion

        #region Setting up a Host
        private static IWebHostBuilder SetingHeaderWebHost(IWebHostBuilder webHost) =>
            webHost.UseKestrel(options => { options.AddServerHeader = false; })
                .UseIISIntegration()
                // Content Root
                .UseContentRoot(Directory.GetCurrentDirectory());

        private static IWebHostBuilder SetingFooterWebHost(IWebHostBuilder webHost) =>
            // Detailed Errors
            webHost.UseSetting("detailedErrors", "true")
                // Capture Startup Errors
                .CaptureStartupErrors(true);
        #endregion
    }
}
