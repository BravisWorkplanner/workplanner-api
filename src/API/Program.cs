using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var logger = services.GetRequiredService<ILogger<Program>>();

                try
                {
                    var forceSeed = ParseCommandlineArgs(args, logger);
                    logger.LogInformation("Attempting to seed database if empty");
                    SeedData.Initialize(services, forceSeed);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "An error occurred seeding the DB.");
                }
            }

            host.Run();
        }

        private static bool ParseCommandlineArgs(string[] args, ILogger logger)
        {
            var forceSeed = false;
            if (args.Length > 1 && bool.TryParse(args[1], out forceSeed))
            {
                logger.LogInformation("Force seed true. Will clear database and re-seed");
            }

            return forceSeed;
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args).ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
    }
}