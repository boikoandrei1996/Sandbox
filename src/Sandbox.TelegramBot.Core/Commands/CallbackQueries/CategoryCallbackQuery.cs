using System.Threading;
using System.Threading.Tasks;
using Sandbox.TelegramBot.Core.DataAccess;
using Sandbox.TelegramBot.Core.Enums;
using Sandbox.TelegramBot.Core.Helpers;
using Sandbox.TelegramBot.Core.Models.CallbackDataModels;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace Sandbox.TelegramBot.Core.Commands.CallbackQueries
{
    public class CategoryCallbackQuery : ICallbackQuery
    {
        internal static readonly CallbackType Type = CallbackType.Category;

        private readonly DocumentIdCache _documentIdCache;

        public CategoryCallbackQuery(
            DocumentIdCache documentIdCache
        )
        {
            _documentIdCache = documentIdCache;
        }

        public string Key => CallbackType.Category.ToString();

        public async Task Handle(ITelegramBotClient botClient, CallbackQuery callbackQuery, CancellationToken cancellationToken)
        {
            await botClient.AnswerCallbackQueryAsync(
                callbackQuery.Id,
                cancellationToken: cancellationToken
            );

            var data = JsonHelper.Deserialize<CategoryCallbackDataModel>(callbackQuery.Data);
            switch (data.Category)
            {
                case CategoryType.Golubika:
                    await GolubikaCategory(botClient, callbackQuery.Message, cancellationToken);
                    break;
                default:
                    await NotFoundCategory(botClient, callbackQuery.Message, cancellationToken);
                    break;
            }
        }

        private async Task GolubikaCategory(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken)
        {
            foreach (var item in DataHelper.CatalogCategories[CategoryType.Golubika].Items)
            {
                /*await botClient.SendCachedPhotoAsync(
                    _documentIdCache,
                    message.Chat,
                    filePath: item.Filepath,
                    caption: $"<b>Сорт:</b> {item.Name}\n<b>Возраст:</b> {item.Age}\n<b>Цена:</b> {item.Price}",
                    parseMode: ParseMode.Html,
                    replyMarkup: BuildDetailsKeyboard(item.DetailsUrl),
                    cancellationToken: cancellationToken
                );*/
                await botClient.SendBlobAsync(
                    message.Chat,
                    BlobType.Photo,
                    item.FileUrl,
                    caption: $"<b>Сорт:</b> {item.Name}\n<b>Возраст:</b> {item.Age}\n<b>Цена:</b> {item.Price}",
                    parseMode: ParseMode.Html,
                    replyMarkup: BuildDetailsKeyboard(item.DetailsUrl),
                    cancellationToken: cancellationToken
                );
            }
        }

        private async Task NotFoundCategory(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken)
        {
            await botClient.SendTextMessageAsync(
                message.Chat,
                "Категория не найдена. Попробуйте ещё раз.",
                cancellationToken: cancellationToken
            );
        }

        private InlineKeyboardMarkup BuildDetailsKeyboard(string url)
        {
            return new InlineKeyboardMarkup(InlineKeyboardButton.WithUrl("Подробнее", url));
        }
    }
}
