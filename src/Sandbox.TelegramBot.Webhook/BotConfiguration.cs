using System;
using Sandbox.TelegramBot.Core;

namespace Sandbox.TelegramBot.Webhook
{
    public class BotConfiguration : BotConfigurationBase
    {
        public const string Id = "Bot";

        protected override void ValidateBotSection()
        {
            base.ValidateBotSection();

            if (string.IsNullOrEmpty(Bot.WebhookBaseAddress))
            {
                throw new InvalidOperationException($"{nameof(Bot)}:{nameof(Bot.WebhookBaseAddress)} is empty");
            }
        }
    }
}
