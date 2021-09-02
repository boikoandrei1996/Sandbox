using System.Threading;
using System.Threading.Tasks;
using Sandbox.TelegramBot.Core.DataAccess;
using Sandbox.TelegramBot.Core.Helpers;
using Sandbox.TelegramBot.Core.Services;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Sandbox.TelegramBot.Core.Commands.RequestInputs
{
    public class SurveyRequestInputCommand : IRequestInput
    {
        private readonly SurveyFormCache _surveyFormCache;
        private readonly HandleSurveyStateService _surveyStateHandler;

        public SurveyRequestInputCommand(
            SurveyFormCache surveyFormCache,
            HandleSurveyStateService surveyStateHandler
        )
        {
            _surveyFormCache = surveyFormCache;
            _surveyStateHandler = surveyStateHandler;
        }

        public string Key => "SurveyRequestInput";

        public async Task Handle(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken)
        {
            await botClient.HandleOneTimeInlineButtonAsync(
                message.Chat,
                message.MessageId - 1,
                null, // message.Text,
                cancellationToken
            );

            var surveyForm = await _surveyFormCache.GetValueAsync(message.Chat, cancellationToken);

            surveyForm!.AddCurrentStateAnswer(message.Text);

            surveyForm.MoveNextState();

            await _surveyStateHandler.HandleSurveyStateAsync(botClient, message, surveyForm, cancellationToken);
        }
    }
}
