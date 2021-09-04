using System.Threading;
using System.Threading.Tasks;
using Sandbox.TelegramBot.Core.Helpers;
using Sandbox.TelegramBot.Core.Models;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Sandbox.TelegramBot.Core.Commands.BotCommands
{
    public class CatalogCommand : IBotCommand
    {
        public static readonly BotCommandInfo Info = new()
        {
            Command = "/catalog",
            AltCommand = "/каталог",
            Description = "Get catalog"
        };

        public string Key => "Catalog";

        public async Task Handle(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken)
        {
            var keyboard = KeyboardHelper.GetCatalogCategoriesKeyboard();

            await botClient.SendTextMessageAsync(
                message.Chat,
                "Выберите категорию:",
                replyMarkup: keyboard,
                cancellationToken: cancellationToken
            );
        }
    }
}
