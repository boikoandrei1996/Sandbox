using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Sandbox.TelegramBot.Core.Models;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace Sandbox.TelegramBot.Core.Commands.BotCommands
{
    public class ContactInfoCommand : IBotCommand
    {
        public static readonly BotCommandInfo Info = new()
        {
            Command = "/contacts",
            AltCommand = "/контакты",
            Description = "Get contacts"
        };

        public string Key => "Contacts";

        public async Task Handle(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken)
        {
            await botClient.SendTextMessageAsync(
                message.Chat,
                BuildContactText(),
                parseMode: ParseMode.Html,
                disableWebPagePreview: true,
                cancellationToken: cancellationToken
            );

            await botClient.SendTextMessageAsync(
                message.Chat,
                "Наши контакты",
                replyMarkup: new InlineKeyboardMarkup(BuildButtons()),
                cancellationToken: cancellationToken
            );

            await botClient.SendContactAsync(
                message.Chat,
                phoneNumber: "+375 (44) 729-92-69",
                firstName: "Руслан",
                cancellationToken: cancellationToken
            );

            await botClient.SendVenueAsync(
                message.Chat,
                latitude: 53.4884245683349f,
                longitude: 27.0715378344647f,
                title: "Офис",
                address: "РБ, 223411, Минская обл., Узденский р-н, Неманский с/с, д. Ракошичи, ул. Центральная, 30",
                cancellationToken: cancellationToken
            );
        }

        private static string BuildContactText()
        {
            static IEnumerable<(string Text, string Link)> GetContactLinks()
            {
                yield return ("Cайт", "https://www.bluecoin.by");
                yield return ("Перейти в Instagram", "https://www.instagram.com/bluecoin.by/");
                yield return ("Перейти в VK", "https://www.vk.com/bluecoinby");
            }

            var contactTextBuilder = new StringBuilder("Контакты:\n");
            foreach (var (Text, Link) in GetContactLinks())
            {
                contactTextBuilder.AppendFormat("<a href='{0}'>{1}</a>\n", Link, Text);
            }

            return contactTextBuilder.ToString();
        }

        private static IEnumerable<InlineKeyboardButton[]> BuildButtons()
        {
            static IEnumerable<(string Text, string Link)> GetContactLinks()
            {
                yield return ("Cайт", "https://www.bluecoin.by");
                yield return ("Перейти в Instagram", "https://www.instagram.com/bluecoin.by/");
                yield return ("Перейти в VK", "https://www.vk.com/bluecoinby");
            }

            foreach (var (Text, Link) in GetContactLinks())
            {
                yield return new[] { InlineKeyboardButton.WithUrl(Text, Link) };
            }
        }
    }
}
