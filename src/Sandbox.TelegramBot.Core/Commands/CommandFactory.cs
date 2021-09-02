using System;
using Microsoft.Extensions.DependencyInjection;
using Sandbox.TelegramBot.Core.Commands.BotCommands;
using Sandbox.TelegramBot.Core.Commands.CallbackQueries;
using Sandbox.TelegramBot.Core.Commands.RequestInputs;
using Sandbox.TelegramBot.Core.DataAccess;
using Sandbox.TelegramBot.Core.Enums;
using Sandbox.TelegramBot.Core.Helpers;
using Sandbox.TelegramBot.Core.Models;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Sandbox.TelegramBot.Core.Commands
{
    public class CommandFactory
    {
        public static readonly UpdateType[] AllowedUpdates = Array.Empty<UpdateType>();

        public static readonly BotCommandInfo[] AllowedCommands = new[]
        {
            ContactInfoCommand.Info,
            CatalogCommand.Info,
            SurveyCommand.Info,
            UsageCommand.Info
        };

        private readonly IServiceProvider _serviceProvider;
        private readonly SurveyFormCache _surveyFormCache;

        public CommandFactory(
            IServiceProvider serviceProvider,
            SurveyFormCache surveyFormCache
        )
        {
            _serviceProvider = serviceProvider;
            _surveyFormCache = surveyFormCache;
        }

        public ICommandHandler<Message> ParseCommand(Message message)
        {
            var requestInput = TryParseRequestInput(message);
            if (requestInput != null)
            {
                return requestInput;
            }

            var botCommand = TryParseBotCommand(message);
            if (botCommand != null)
            {
                return botCommand;
            }

            return _serviceProvider.GetRequiredService<UsageCommand>();
        }

        public ICommandHandler<CallbackQuery> ParseCallbackQuery(CallbackQuery callbackQuery)
        {
            var data = JsonHelper.DeserializeAnonymousType(callbackQuery.Data, new { Type = default(CallbackType) });

            if (CategoryCallbackQuery.Type == data.Type)
            {
                return _serviceProvider.GetRequiredService<CategoryCallbackQuery>();
            }

            if (SurveyStartCallbackQuery.Type == data.Type)
            {
                return _serviceProvider.GetRequiredService<SurveyStartCallbackQuery>();
            }

            if (SurveyEndCallbackQuery.Type == data.Type)
            {
                return _serviceProvider.GetRequiredService<SurveyEndCallbackQuery>();
            }

            if (SurveyQuestionCallbackQuery.Type == data.Type)
            {
                return _serviceProvider.GetRequiredService<SurveyQuestionCallbackQuery>();
            }

            throw new NotImplementedException(nameof(CallbackType));
        }

        private IBotCommand? TryParseBotCommand(Message message)
        {
            var text = message.Text.AsSpan();
            var index = text.IndexOf(' ');
            var commandName = index == -1 ? text : text.Slice(0, index);

            if (ValidateCommand(commandName, ContactInfoCommand.Info))
            {
                return _serviceProvider.GetRequiredService<ContactInfoCommand>();
            }

            if (ValidateCommand(commandName, CatalogCommand.Info))
            {
                return _serviceProvider.GetRequiredService<CatalogCommand>();
            }

            if (ValidateCommand(commandName, SurveyCommand.Info))
            {
                return _serviceProvider.GetRequiredService<SurveyCommand>();
            }

            if (ValidateCommand(commandName, UsageCommand.Info))
            {
                return _serviceProvider.GetRequiredService<UsageCommand>();
            }

            return null;
        }

        private IRequestInput? TryParseRequestInput(Message message)
        {
            var surveyForm = _surveyFormCache.GetValue(message.Chat);
            if (surveyForm is not null && surveyForm.IsRequestInputState)
            {
                return _serviceProvider.GetRequiredService<SurveyRequestInputCommand>();
            }

            return null;
        }

        private bool ValidateCommand(ReadOnlySpan<char> commandName, BotCommandInfo commandInfo)
        {
            var comparisonType = StringComparison.Ordinal;
            // var comparisonType = StringComparison.OrdinalIgnoreCase;

            return
                commandName.Equals(commandInfo.Command, comparisonType) ||
                commandName.Equals(commandInfo.AltCommand, comparisonType);
        }
    }
}
