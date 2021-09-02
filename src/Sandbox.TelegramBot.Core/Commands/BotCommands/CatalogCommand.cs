using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Sandbox.TelegramBot.Core.Helpers;
using Sandbox.TelegramBot.Core.Models;
using Sandbox.TelegramBot.Core.Models.CallbackDataModels;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

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
            var buttons = DataHelper.CatalogCategories
                .Select(keyValue =>
                    InlineKeyboardButton.WithCallbackData(
                        keyValue.Value.Title,
                        new CategoryCallbackDataModel(keyValue.Key).Serialize()
                    )
                )
                .ToArray();
            var keyboard = new InlineKeyboardMarkup(buttons);

            await botClient.SendTextMessageAsync(
                message.Chat,
                "Выберите категорию:",
                replyMarkup: keyboard,
                cancellationToken: cancellationToken
            );
        }
    }
}
