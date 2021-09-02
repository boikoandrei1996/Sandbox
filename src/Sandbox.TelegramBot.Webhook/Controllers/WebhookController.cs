using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Sandbox.TelegramBot.Core.Services;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Sandbox.TelegramBot.Webhook.Controllers
{
    public class WebhookController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Post(
            [FromServices] ITelegramBotClient botClient,
            [FromServices] HandleBotUpdateService updateHandler,
            [FromBody] Update update,
            CancellationToken cancellationToken
        )
        {
            try
            {
                await updateHandler.HandleUpdateAsync(botClient, update, cancellationToken);
            }
            catch (OperationCanceledException)
            {
                // ignored
            }
            catch (Exception ex)
            {
                try
                {
                    await updateHandler.HandleErrorAsync(botClient, ex, cancellationToken);
                }
                catch (OperationCanceledException)
                {
                    // ignored
                }
            }

            return Ok();
        }
    }
}
