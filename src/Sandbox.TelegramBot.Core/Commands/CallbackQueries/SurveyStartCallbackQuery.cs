using System.Threading;
using System.Threading.Tasks;
using Sandbox.TelegramBot.Core.Enums;
using Sandbox.TelegramBot.Core.Helpers;
using Sandbox.TelegramBot.Core.Models;
using Sandbox.TelegramBot.Core.Services;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Sandbox.TelegramBot.Core.Commands.CallbackQueries
{
    public class SurveyStartCallbackQuery : ICallbackQuery
    {
        internal static readonly CallbackType Type = CallbackType.SurveyStart;

        private readonly HandleSurveyStateService _surveyStateHandler;

        public SurveyStartCallbackQuery(
            HandleSurveyStateService surveyStateHandler
        )
        {
            _surveyStateHandler = surveyStateHandler;
        }

        public string Key => CallbackType.SurveyStart.ToString();

        public async Task Handle(ITelegramBotClient botClient, CallbackQuery callbackQuery, CancellationToken cancellationToken)
        {
            var message = callbackQuery.Message;

            await botClient.AnswerCallbackQueryAsync(
                callbackQuery.Id,
                cancellationToken: cancellationToken
            );

            await botClient.HandleOneTimeInlineButtonAsync(
                message.Chat,
                message.MessageId,
                null,
                cancellationToken
            );

            var surveyForm = new SurveyForm();

            surveyForm.MoveNextState();

            await _surveyStateHandler.HandleSurveyStateAsync(botClient, message, surveyForm, cancellationToken);
        }
    }
}
