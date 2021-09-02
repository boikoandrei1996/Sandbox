using System;
using System.Threading;
using System.Threading.Tasks;
using Sandbox.TelegramBot.Core.DataAccess;
using Sandbox.TelegramBot.Core.Enums;
using Sandbox.TelegramBot.Core.Helpers;
using Sandbox.TelegramBot.Core.Models.CallbackDataModels;
using Sandbox.TelegramBot.Core.Services;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Sandbox.TelegramBot.Core.Commands.CallbackQueries
{
    public class SurveyQuestionCallbackQuery : ICallbackQuery
    {
        internal static readonly CallbackType Type = CallbackType.SurveyQuestion;

        private readonly SurveyFormCache _surveyFormCache;
        private readonly HandleSurveyStateService _surveyStateHandler;

        public SurveyQuestionCallbackQuery(
            SurveyFormCache surveyFormCache,
            HandleSurveyStateService surveyStateHandler
        )
        {
            _surveyFormCache = surveyFormCache;
            _surveyStateHandler = surveyStateHandler;
        }

        public string Key => CallbackType.SurveyQuestion.ToString();

        public async Task Handle(ITelegramBotClient botClient, CallbackQuery callbackQuery, CancellationToken cancellationToken)
        {
            var message = callbackQuery.Message;

            var data = JsonHelper.Deserialize<SurveyQuestionCallbackDataModel>(callbackQuery.Data);
            var answer = DataHelper.SurveyQuestions[data.Question].Answers[data.AnswerId].Text;

            await botClient.AnswerCallbackQueryAsync(
                callbackQuery.Id,
                cancellationToken: cancellationToken
            );

            await botClient.HandleOneTimeInlineButtonAsync(
                message.Chat,
                message.MessageId,
                answer,
                cancellationToken
            );

            var surveyForm = await _surveyFormCache.GetValueAsync(message.Chat, cancellationToken);
            if (surveyForm is null)
            {
                throw new InvalidOperationException("SurveyForm is not found in cache.");
            }

            surveyForm.AddCurrentStateAnswer(answer);

            surveyForm.MoveNextState(surveyForm.IsQuestionState && data.AnswerId == Constants.CustomAnswerId);

            await _surveyStateHandler.HandleSurveyStateAsync(botClient, message, surveyForm, cancellationToken);
        }
    }
}
