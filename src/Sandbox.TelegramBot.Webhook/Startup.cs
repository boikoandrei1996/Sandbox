using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using Sandbox.TelegramBot.Core.Helpers;
using Sandbox.TelegramBot.Webhook.HealthProbes;
using Sandbox.TelegramBot.Webhook.Services;
using Serilog;
using Serilog.Events;

namespace Sandbox.TelegramBot.Webhook
{
    public class Startup
    {
        private readonly BotConfiguration _botConfig;

        public Startup(IConfiguration configuration)
        {
            _botConfig = configuration.GetSection(BotConfiguration.Id).Get<BotConfiguration>();
            _botConfig.Validate();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddLogging(configure =>
                {
                    var logger = new LoggerConfiguration()
                        .MinimumLevel.Debug()
                        .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                        .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
                        .Enrich.FromLogContext()
                        .WriteTo.Console()
                        .CreateLogger();

                    configure.ClearProviders();
                    configure.AddSerilog(logger);
                })
                .AddTelegramBotClient(_botConfig.Bot)
                .AddDbAccess(_botConfig.Db)
                .AddCacheAccess(_botConfig.Cache)
                .AddCommands()
                .AddServices()
                .AddSingleton(_botConfig)
                .AddHostedService<ConfigureWebhookService>();

            services
                .AddHealthChecks()
                .AddCheck<BotClientProbe>(BotClientProbe.Name, timeout: BotClientProbe.Timeout)
                .AddCheck<MongoDbProbe>(MongoDbProbe.Name, timeout: BotClientProbe.Timeout)
                .AddCheck<RedisCacheProbe>(RedisCacheProbe.Name, timeout: BotClientProbe.Timeout);

            // The Telegram.Bot library heavily depends on Newtonsoft.Json library to deserialize
            // incoming webhook updates and send serialized responses back.
            services
                .AddControllers()
                .AddNewtonsoftJson();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseRouting();
            // app.UseCors();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHealthChecks("/health", new()
                {
                    ResponseWriter = WriteHealthProbeHtmlResponse
                });

                // Configure custom endpoint per Telegram API recommendations: https://core.telegram.org/bots/api#setwebhook
                // If you'd like to make sure that the Webhook request comes from Telegram,
                // we recommend using a secret path in the URL, e.g. https://www.example.com/<token>.
                // Since nobody else knows your bot's token, you can be pretty sure it's us.
                endpoints.MapControllerRoute(
                    name: "tgwebhook",
                    pattern: $"bot/{_botConfig.Bot.Token}",
                    defaults: new { controller = "Webhook", action = "Post" }
                );
                endpoints.MapControllers();
            });
        }

        private static Task WriteHealthProbeHtmlResponse(HttpContext context, HealthReport result)
        {
            var resultBuilder = new StringBuilder();

            resultBuilder.Append($"<div>Status - {GetHtml(result.Status)}</div>");

            resultBuilder.Append("<ul>");
            foreach (var (key, entry) in result.Entries)
            {
                resultBuilder.Append($"<li>{key} - {GetHtml(entry.Status)}</li>");
            }
            resultBuilder.Append("</ul>");

            static string GetHtml(HealthStatus status)
            {
                var color = status switch
                {
                    HealthStatus.Healthy => "green",
                    HealthStatus.Degraded => "yellow",
                    HealthStatus.Unhealthy => "red",
                    _ => throw new NotImplementedException(nameof(HealthStatus))
                };

                return $"<strong style='color: {color};'>{status}</strong>";
            }

            return context.Response.WriteAsync(resultBuilder.ToString());
        }
    }
}
