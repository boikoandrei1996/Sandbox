using System;
using System.Threading;
using System.Threading.Tasks;
using Sandbox.TelegramBot.Core.Enums;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InputFiles;
using Telegram.Bot.Types.ReplyMarkups;

namespace Sandbox.TelegramBot.Core.Helpers
{
    internal static class TelegramBotClientExtensions
    {
        public static async Task HandleOneTimeInlineButtonAsync(
            this ITelegramBotClient botClient,
            ChatId chatId,
            int messageId,
            string? answerText,
            CancellationToken cancellationToken = default)
        {
            await botClient.EditMessageReplyMarkupAsync(
                chatId,
                messageId,
                InlineKeyboardMarkup.Empty(),
                cancellationToken: cancellationToken
            );

            if (answerText is not null)
            {
                await botClient.SendTextMessageAsync(
                    chatId,
                    $"Ваш ответ: {Constants.YourAnswerEmoji} {answerText}",
                    cancellationToken: cancellationToken
                );
            }
        }

        public static async Task SendBlobAsync(
            this ITelegramBotClient botClient,
            ChatId chatId,
            BlobType blobType,
            string blobUrl,
            string? caption = null,
            ParseMode parseMode = ParseMode.Default,
            bool disableNotification = false,
            IReplyMarkup? replyMarkup = null,
            CancellationToken cancellationToken = default)
        {
            var file = new InputOnlineFile(new Uri(blobUrl));

            switch (blobType)
            {
                case BlobType.Photo:
                    await botClient.SendPhotoAsync(
                        chatId,
                        photo: file,
                        caption: caption,
                        parseMode: parseMode,
                        disableNotification: disableNotification,
                        replyMarkup: replyMarkup,
                        cancellationToken: cancellationToken
                    );
                    break;
                case BlobType.Document:
                    await botClient.SendDocumentAsync(
                        chatId,
                        document: file,
                        caption: caption,
                        parseMode: parseMode,
                        disableNotification: disableNotification,
                        replyMarkup: replyMarkup,
                        cancellationToken: cancellationToken
                    );
                    break;
                default:
                    throw new NotImplementedException(nameof(BlobType));
            }
        }

        /*public static async Task SendCachedBlobAsync(
            this ITelegramBotClient botClient,
            DocumentIdCache cache,
            ChatId chatId,
            string fileUrl,
            bool isPhoto,
            string? caption = null,
            ParseMode parseMode = ParseMode.Default,
            bool disableNotification = false,
            IReplyMarkup? replyMarkup = null,
            CancellationToken cancellationToken = default)
        {
            var cacheKey = GetHash(fileUrl);
            
            var fileId = await cache.GetValueAsync(cacheKey, cancellationToken);

            var fileIdOrUrl = fileId is null ? fileUrl : fileId;

            var result = isPhoto ? 
                await botClient.SendPhotoAsync(
                    chatId,
                    photo: new InputOnlineFile(fileIdOrUrl),
                    caption: caption,
                    parseMode: parseMode,
                    disableNotification: disableNotification,
                    replyMarkup: replyMarkup,
                    cancellationToken: cancellationToken
                ) :
                await botClient.SendDocumentAsync(
                    chatId,
                    document: new InputOnlineFile(fileIdOrUrl),
                    caption: caption,
                    parseMode: parseMode,
                    disableNotification: disableNotification,
                    replyMarkup: replyMarkup,
                    cancellationToken: cancellationToken
                );

            if (fileId is null)
            {
                fileId = isPhoto ? result.Photo.First().FileId : result.Document.FileId;

                await cache.SetValueAsync(cacheKey, fileId, cancellationToken);
            }
        }*/

        /*public static async Task SendCachedPhotoAsync(
            this ITelegramBotClient botClient,
            DocumentIdCache documentIdStore,
            ChatId chatId,
            string filePath,
            string? caption = null,
            ParseMode parseMode = ParseMode.Default,
            bool disableNotification = false,
            IReplyMarkup? replyMarkup = null,
            CancellationToken cancellationToken = default)
        {
            var cacheKey = GetHash(filePath);

            var fileId = await documentIdStore.GetValueAsync(cacheKey, cancellationToken);

            if (fileId is null)
            {
                var filename = Path.GetFileName(filePath);

                using (var stream = IOFile.OpenRead(filePath))
                {
                    var result = await botClient.SendPhotoAsync(
                        chatId,
                        photo: new InputOnlineFile(stream, filename),
                        caption: caption,
                        parseMode: parseMode,
                        disableNotification: disableNotification,
                        replyMarkup: replyMarkup,
                        cancellationToken: cancellationToken
                    );

                    await documentIdStore.SetValueAsync(cacheKey, result.Photo.First().FileId, cancellationToken);
                }
            }
            else
            {
                await botClient.SendPhotoAsync(
                    chatId,
                    photo: new InputOnlineFile(fileId),
                    caption: caption,
                    parseMode: parseMode,
                    disableNotification: disableNotification,
                    replyMarkup: replyMarkup,
                    cancellationToken: cancellationToken
                );
            }
        }

        public static async Task SendCachedDocumentAsync(
            this ITelegramBotClient botClient,
            DocumentIdCache documentIdStore,
            ChatId chatId,
            string filePath,
            string? caption = null,
            ParseMode parseMode = ParseMode.Default,
            bool disableNotification = false,
            IReplyMarkup? replyMarkup = null,
            CancellationToken cancellationToken = default)
        {
            var cacheKey = GetHash(filePath);

            var fileId = await documentIdStore.GetValueAsync(cacheKey, cancellationToken);

            if (fileId is null)
            {
                var filename = Path.GetFileName(filePath);

                using (var stream = IOFile.OpenRead(filePath))
                {
                    var result = await botClient.SendDocumentAsync(
                        chatId,
                        document: new InputOnlineFile(stream, filename),
                        caption: caption,
                        parseMode: parseMode,
                        disableNotification: disableNotification,
                        replyMarkup: replyMarkup,
                        cancellationToken: cancellationToken
                    );

                    await documentIdStore.SetValueAsync(cacheKey, result.Document.FileId, cancellationToken);
                }
            }
            else
            {
                await botClient.SendDocumentAsync(
                    chatId,
                    document: new InputOnlineFile(fileId),
                    caption: caption,
                    parseMode: parseMode,
                    disableNotification: disableNotification,
                    replyMarkup: replyMarkup,
                    cancellationToken: cancellationToken
                );
            }
        }*/

        /*private static string GetHash(string input)
        {
            byte[] data;
            using (var hashAlgorithm = SHA256.Create())
            {
                data = hashAlgorithm.ComputeHash(Encoding.UTF8.GetBytes(input));
            }

            var builder = new StringBuilder(data.Length * 2);

            // Loop through each byte of the hashed data
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                builder.Append(data[i].ToString("x2"));
            }

            return builder.ToString();
        }*/
    }
}
