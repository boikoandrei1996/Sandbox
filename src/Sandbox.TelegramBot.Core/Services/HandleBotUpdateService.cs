using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Sandbox.TelegramBot.Core.Commands;
using Sandbox.TelegramBot.Core.DataAccess;
using Sandbox.TelegramBot.Core.Models.Documents;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Sandbox.TelegramBot.Core.Services
{
    public class HandleBotUpdateService
    {
        private readonly ILogger _logger;
        private readonly CommandFactory _commandFactory;
        private readonly HistoryRepository _historyRepository;

        public HandleBotUpdateService(
            ILogger<HandleBotUpdateService> logger,
            CommandFactory commandFactory,
            HistoryRepository historyRepository
        )
        {
            _logger = logger;
            _commandFactory = commandFactory;
            _historyRepository = historyRepository;
        }

        public Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            var errorMessage = exception switch
            {
                ApiRequestException apiRequestException => $"Telegram API Error: [{apiRequestException.ErrorCode}] {apiRequestException.Message}",
                _ => exception.ToString()
            };

            _logger.LogError(exception, errorMessage);

            return Task.CompletedTask;
        }

        public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            var handler = update.Type switch
            {
                UpdateType.Message => OnMessageReceived(botClient, update.Message, cancellationToken),
                UpdateType.CallbackQuery => OnCallbackQueryReceived(botClient, update.CallbackQuery, cancellationToken),
                _ => OnUnsupportedUpdateTypeReceived(botClient, update, cancellationToken)
            };

            try
            {
                await handler;
            }
            catch (Exception exception)
            {
                await HandleErrorAsync(botClient, exception, cancellationToken);
            }
        }

        private async Task OnMessageReceived(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken)
        {
            if (message.Type != MessageType.Text)
            {
                _logger.LogWarning($"Unsupported message type: {message.Type}");
                return;
            }

            _logger.LogInformation(message.Text);

            var command = _commandFactory.ParseCommand(message);

            var historyAction = new HistoryAction
            {
                Action = command.Key,
                Data = message.Text,
                ActionedAt = DateTime.UtcNow
            };

            await Task.WhenAll(
                command.Handle(botClient, message, cancellationToken),
                PushHistoryActionAsync(message.Chat, historyAction, cancellationToken)
            );
        }

        private async Task OnCallbackQueryReceived(ITelegramBotClient botClient, CallbackQuery callbackQuery, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(callbackQuery.Data))
            {
                _logger.LogWarning($"Callback query data is empty.");
                return;
            }

            _logger.LogInformation(callbackQuery.Data);
            _logger.LogInformation($"Byte size: {System.Text.Encoding.Default.GetByteCount(callbackQuery.Data)}");

            var command = _commandFactory.ParseCallbackQuery(callbackQuery);

            var historyAction = new HistoryAction
            {
                Action = command.Key,
                Data = callbackQuery.Data,
                ActionedAt = DateTime.UtcNow
            };

            await Task.WhenAll(
                command.Handle(botClient, callbackQuery, cancellationToken),
                PushHistoryActionAsync(callbackQuery.Message.Chat, historyAction, cancellationToken)
            );
        }

        private Task OnUnsupportedUpdateTypeReceived(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            _logger.LogWarning($"Unsupported update type: {update.Type}");

            return Task.CompletedTask;
        }

        private Task PushHistoryActionAsync(Chat chat, HistoryAction action, CancellationToken cancellationToken)
        {
            return _historyRepository.PushAsync(
                chat.Id.ToString(),
                action,
                cancellationToken
            );
        }
    }
}
