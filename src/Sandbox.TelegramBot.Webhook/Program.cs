using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Sandbox.TelegramBot.Webhook
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            using (var host = CreateHostBuilder(args).Build())
            {
                var logger = host.Services.GetRequiredService<ILogger<Program>>();

                try
                {
                    logger.LogInformation("Starting web host");
                    await host.RunAsync();
                }
                catch (Exception ex)
                {
                    logger.LogCritical(ex, "Host terminated unexpectedly");
                }
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
