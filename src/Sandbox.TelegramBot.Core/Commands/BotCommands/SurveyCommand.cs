using System.Threading;
using System.Threading.Tasks;
using Sandbox.TelegramBot.Core.Enums;
using Sandbox.TelegramBot.Core.Models;
using Sandbox.TelegramBot.Core.Models.CallbackDataModels;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace Sandbox.TelegramBot.Core.Commands.BotCommands
{
    public class SurveyCommand : IBotCommand
    {
        public static readonly BotCommandInfo Info = new()
        {
            Command = "/survey",
            AltCommand = "/опрос",
            Description = "Take survey"
        };

        public string Key => "Survey";

        public async Task Handle(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken)
        {
            var surveyInfoText =
                "Ответьте на 4 простых вопроса и получите наиболее выгодное предложение\n" +
                "В конце опроса вы получите полный каталог продукции лучшего посадочного материала и скидку на первую отгрузку";

            var buttons = new[]
            {
                InlineKeyboardButton.WithCallbackData(
                    "Начнём",
                    new SurveyActionCallbackDataModel(CallbackType.SurveyStart).Serialize()
                ),
                InlineKeyboardButton.WithCallbackData(
                    "Нет, спасибо",
                    new SurveyActionCallbackDataModel(CallbackType.SurveyEnd).Serialize()
                )
            };
            var keyboard = new InlineKeyboardMarkup(buttons);

            await botClient.SendTextMessageAsync(
                message.Chat,
                surveyInfoText,
                replyMarkup: keyboard,
                cancellationToken: cancellationToken
            );
        }
    }
}
