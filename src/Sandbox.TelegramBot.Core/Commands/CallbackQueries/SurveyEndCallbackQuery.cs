using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Sandbox.TelegramBot.Core.Commands.BotCommands;
using Sandbox.TelegramBot.Core.DataAccess;
using Sandbox.TelegramBot.Core.Enums;
using Sandbox.TelegramBot.Core.Helpers;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Sandbox.TelegramBot.Core.Commands.CallbackQueries
{
    public class SurveyEndCallbackQuery : ICallbackQuery
    {
        internal static readonly CallbackType Type = CallbackType.SurveyEnd;

        private readonly SurveyFormCache _surveyFormCache;
        private readonly UsageCommand _usageCommand;

        public SurveyEndCallbackQuery(
            SurveyFormCache surveyFormCache,
            UsageCommand usageCommand
        )
        {
            _surveyFormCache = surveyFormCache;
            _usageCommand = usageCommand;
        }

        public string Key => CallbackType.SurveyEnd.ToString();

        public async Task Handle(ITelegramBotClient botClient, CallbackQuery callbackQuery, CancellationToken cancellationToken)
        {
            var message = callbackQuery.Message;

            var clickedButton = message.ReplyMarkup.InlineKeyboard
                .SelectMany(x => x)
                .First(btn => btn.CallbackData == callbackQuery.Data);

            await botClient.AnswerCallbackQueryAsync(
                callbackQuery.Id,
                cancellationToken: cancellationToken
            );

            await botClient.HandleOneTimeInlineButtonAsync(
                message.Chat,
                message.MessageId,
                clickedButton.Text,
                cancellationToken
            );

            await Task.WhenAll(
                _usageCommand.Handle(botClient, message, cancellationToken),
                _surveyFormCache.RemoveValueAsync(message.Chat, cancellationToken)
            );
        }
    }
}
