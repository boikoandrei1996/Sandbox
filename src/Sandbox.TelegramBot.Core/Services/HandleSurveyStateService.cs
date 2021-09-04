using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Sandbox.TelegramBot.Core.Commands.BotCommands;
using Sandbox.TelegramBot.Core.DataAccess;
using Sandbox.TelegramBot.Core.Enums;
using Sandbox.TelegramBot.Core.Helpers;
using Sandbox.TelegramBot.Core.Models;
using Sandbox.TelegramBot.Core.Models.CallbackDataModels;
using Sandbox.TelegramBot.Core.Models.Documents;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace Sandbox.TelegramBot.Core.Services
{
    public class HandleSurveyStateService
    {
        private readonly SurveyFormCache _surveyFormCache;
        private readonly DocumentIdCache _documentIdCache;
        private readonly SurveyAnswerRepository _surveyAnswerRepository;
        private readonly UsageCommand _usageCommand;

        public HandleSurveyStateService(
            SurveyFormCache surveyFormCache,
            DocumentIdCache documentIdCache,
            SurveyAnswerRepository surveyAnswerRepository,
            UsageCommand usageCommand
        )
        {
            _surveyFormCache = surveyFormCache;
            _documentIdCache = documentIdCache;
            _surveyAnswerRepository = surveyAnswerRepository;
            _usageCommand = usageCommand;
        }

        public async Task HandleSurveyStateAsync(ITelegramBotClient botClient, Message message, SurveyForm surveyForm, CancellationToken cancellationToken)
        {
            var handler = surveyForm.CurrentState switch
            {
                SurveyState.None => HandleSurveyFinish(botClient, message, surveyForm, cancellationToken),
                SurveyState.Question1 => HandleSurveyQuestionAsk(botClient, message, SurveyQuestionType.Question1, cancellationToken),
                SurveyState.Question2 => HandleSurveyQuestionAsk(botClient, message, SurveyQuestionType.Question2, cancellationToken),
                SurveyState.Question3 => HandleSurveyQuestionAsk(botClient, message, SurveyQuestionType.Question3, cancellationToken),
                SurveyState.Question4 => HandleSurveyQuestionAsk(botClient, message, SurveyQuestionType.Question4, cancellationToken),
                SurveyState.RequestDetailsQuestion1 => HandleSurveyInputRequest(botClient, message, RequestInputType.RequestDetailsQuestion1, cancellationToken),
                SurveyState.RequestDetailsQuestion2 => HandleSurveyInputRequest(botClient, message, RequestInputType.RequestDetailsQuestion2, cancellationToken),
                SurveyState.RequestDetailsQuestion4 => HandleSurveyInputRequest(botClient, message, RequestInputType.RequestDetailsQuestion4, cancellationToken),
                SurveyState.RequestName => HandleSurveyInputRequest(botClient, message, RequestInputType.RequestName, cancellationToken),
                SurveyState.RequestEmail => HandleSurveyInputRequest(botClient, message, RequestInputType.RequestEmail, cancellationToken),
                SurveyState.RequestPhone => HandleSurveyInputRequest(botClient, message, RequestInputType.RequestPhone, cancellationToken),
                _ => throw new NotImplementedException(nameof(SurveyState))
            };

            await Task.WhenAll(
                handler,
                _surveyFormCache.SetValueAsync(message.Chat, surveyForm, cancellationToken)
            );
        }

        private async Task HandleSurveyQuestionAsk(ITelegramBotClient botClient, Message message, SurveyQuestionType questionType, CancellationToken cancellationToken)
        {
            var question = DataHelper.SurveyQuestions[questionType];

            var buttons = question.Answers
                .Select(keyValue => new[]
                {
                    InlineKeyboardButton.WithCallbackData(
                        keyValue.Value.Text,
                        new SurveyQuestionCallbackDataModel(questionType, keyValue.Key).Serialize()
                    )
                })
                .Append(new[]
                {
                    InlineKeyboardButton.WithCallbackData(
                        "Закончить опрос",
                        new SurveyActionCallbackDataModel(CallbackType.SurveyEnd).Serialize()
                    )
                })
                .ToArray();

            var keyboard = new InlineKeyboardMarkup(buttons);

            await botClient.SendTextMessageAsync(
                message.Chat,
                question.Text,
                replyMarkup: keyboard,
                cancellationToken: cancellationToken
            );
        }

        private async Task HandleSurveyInputRequest(ITelegramBotClient botClient, Message message, RequestInputType inputType, CancellationToken cancellationToken)
        {
            var text = inputType switch
            {
                RequestInputType.RequestName => "Ваше имя?",
                RequestInputType.RequestEmail => "Ваш эмейл?",
                RequestInputType.RequestPhone => "Ваш телефон?",
                _ => "Введите:"
            };

            var buttons = new[]
            {
                InlineKeyboardButton.WithCallbackData(
                    "Закончить опрос",
                    new SurveyActionCallbackDataModel(CallbackType.SurveyEnd).Serialize()
                )
            };

            var keyboard = new InlineKeyboardMarkup(buttons);

            await botClient.SendTextMessageAsync(
                message.Chat,
                text,
                replyMarkup: keyboard,
                cancellationToken: cancellationToken
            );
        }

        private async Task HandleSurveyFinish(ITelegramBotClient botClient, Message message, SurveyForm surveyForm, CancellationToken cancellationToken)
        {
            await botClient.SendTextMessageAsync(
                message.Chat,
                "Спасибо, опрос завершён.",
                cancellationToken: cancellationToken
            );

            /*await botClient.SendCachedDocumentAsync(
                _documentIdCache,
                message.Chat,
                @"C:\Users\BoikoAndrei\Downloads\bluecoin\каталог.pdf",
                cancellationToken: cancellationToken
            );*/
            await botClient.SendBlobAsync(
                message.Chat,
                BlobType.Document,
                "https://sandboxtgbotstorage.blob.core.windows.net/bluecoin-documents/каталог.pdf",
                cancellationToken: cancellationToken
            );

            await _usageCommand.Handle(botClient, message, cancellationToken);

            var document = SurveyAnswerDocument.From(surveyForm);
            await _surveyAnswerRepository.CreateAsync(document, cancellationToken);
        }
    }
}
