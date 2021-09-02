using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Sandbox.TelegramBot.Core.Commands;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Sandbox.TelegramBot.Webhook.Services
{
    // There are several strategies for completing asynchronous tasks during startup.
    // Some of them could be found in this article https://andrewlock.net/running-async-tasks-on-app-startup-in-asp-net-core-part-1/
    public class ConfigureWebhookService : IHostedService
    {
        private readonly ITelegramBotClient _botClient;
        private readonly BotConfiguration.BotSection _botSection;

        public ConfigureWebhookService(
            ITelegramBotClient botClient,
            BotConfiguration botConfig
        )
        {
            _botClient = botClient;
            _botSection = botConfig.Bot;
        }

        // Configure custom endpoint per Telegram API recommendations: https://core.telegram.org/bots/api#setwebhook
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var webhookUrl = @$"{_botSection.WebhookBaseAddress}/bot/{_botSection.Token}";

            var webhookInfo = await _botClient.GetWebhookInfoAsync(cancellationToken);
            if (webhookInfo.Url != webhookUrl)
            {
                if (!string.IsNullOrEmpty(webhookInfo.Url))
                {
                    await _botClient.DeleteWebhookAsync(cancellationToken: cancellationToken);
                }

                await _botClient.SetWebhookAsync(
                    url: webhookUrl,
                    allowedUpdates: CommandFactory.AllowedUpdates,
                    cancellationToken: cancellationToken
                );
            }

            var allowedCommands = CommandFactory.AllowedCommands
                .Select(info => new BotCommand
                {
                    Command = info.Command,
                    Description = info.Description
                })
                .ToArray();

            await _botClient.SetMyCommandsAsync(allowedCommands, cancellationToken);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await _botClient.DeleteWebhookAsync(cancellationToken: cancellationToken);
        }
    }
}
