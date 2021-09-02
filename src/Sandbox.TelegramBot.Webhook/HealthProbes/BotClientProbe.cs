using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Telegram.Bot;

namespace Sandbox.TelegramBot.Webhook.HealthProbes
{
    public class BotClientProbe : IHealthCheck
    {
        public const string Name = "bot_client_probe";

        public static readonly TimeSpan? Timeout = TimeSpan.FromSeconds(5);

        private readonly ITelegramBotClient _botClient;

        public BotClientProbe(ITelegramBotClient botClient)
        {
            _botClient = botClient;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _botClient.GetMeAsync(cancellationToken);

                return result is null ?
                    HealthCheckResult.Degraded("User not found") :
                    HealthCheckResult.Healthy("Success");
            }
            catch
            {
                return HealthCheckResult.Unhealthy("Failed");
            }
        }
    }
}
