using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Sandbox.TelegramBot.Core.Models;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace Sandbox.TelegramBot.Core.Commands.BotCommands
{
    public class UsageCommand : IBotCommand
    {
        public static readonly BotCommandInfo Info = new()
        {
            Command = "/usage",
            AltCommand = "/команды",
            Description = "Commands list"
        };

        public string Key => "Usage";

        public async Task Handle(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken)
        {
            var buttons = CommandFactory.AllowedCommands.Select(info => new KeyboardButton(info.AltCommand)).ToArray();
            var keyboard = new ReplyKeyboardMarkup(buttons, resizeKeyboard: true, oneTimeKeyboard: true);

            await botClient.SendTextMessageAsync(
                message.Chat,
                "Выберите:",
                replyMarkup: keyboard,
                cancellationToken: cancellationToken
            );
        }
    }
}
