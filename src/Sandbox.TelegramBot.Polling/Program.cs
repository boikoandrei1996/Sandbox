using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sandbox.TelegramBot.Core.Commands;
using Sandbox.TelegramBot.Core.Helpers;
using Sandbox.TelegramBot.Core.Services;
using Serilog;
using Telegram.Bot;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;

namespace Sandbox.TelegramBot.Polling
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var configuration = BuildConfiguration();

            var botConfig = configuration.GetSection(BotConfiguration.Id).Get<BotConfiguration>();
            botConfig.Validate();

            var services = new ServiceCollection();

            services
                .AddLogging(configure =>
                {
                    var logger = new LoggerConfiguration()
                        .MinimumLevel.Debug()
                        .Enrich.FromLogContext()
                        .WriteTo.Console()
                        .CreateLogger();

                    configure.AddSerilog(logger, dispose: true);
                })
                .AddTelegramBotClient(botConfig.Bot)
                .AddDbAccess(botConfig.Db)
                .AddCacheAccess(botConfig.Cache)
                .AddCommands()
                .AddServices();

            using var serviceProvider = services.BuildServiceProvider();

            var bot = serviceProvider.GetRequiredService<ITelegramBotClient>();
            var botUpdateService = serviceProvider.GetRequiredService<HandleBotUpdateService>();

            var handler = new DefaultUpdateHandler(
                botUpdateService.HandleUpdateAsync,
                botUpdateService.HandleErrorAsync,
                CommandFactory.AllowedUpdates
            );

            using (var cts = new CancellationTokenSource())
            {
                await BeforeStartPolling(bot, cts.Token);

                bot.StartReceiving(handler, cts.Token);

                Console.WriteLine("Start polling...");
                Console.ReadLine();
                cts.Cancel();
            }

            Console.WriteLine("Finish...");
        }

        private static async Task BeforeStartPolling(ITelegramBotClient bot, CancellationToken cancellationToken)
        {
            await bot.DeleteWebhookAsync(cancellationToken: cancellationToken);

            var allowedCommands = CommandFactory.AllowedCommands
                .Select(info => new BotCommand
                {
                    Command = info.Command,
                    Description = info.Description
                })
                .ToArray();

            await bot.SetMyCommandsAsync(allowedCommands, cancellationToken);
        }

        private static IConfiguration BuildConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile("appsettings.Development.json", optional: false, reloadOnChange: true);

            // var env = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT");
            // builder.AddJsonFile($"appsettings.{env}.json", optional: true, reloadOnChange: true);

            return builder.Build();

        }
    }
}
