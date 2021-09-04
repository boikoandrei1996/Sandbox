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
                case CategoryType.Brusnika:
                case CategoryType.Klukva:
                case CategoryType.Zhimolosti:
                    await EmptyCategory(botClient, callbackQuery.Message, cancellationToken);
                    break;
                default:
                    await NotFoundCategory(botClient, callbackQuery.Message, cancellationToken);
                    break;
            }
        }

        private async Task GolubikaCategory(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken)
        {
            var items = DataHelper.CatalogCategories[CategoryType.Golubika].Items;
            foreach (var item in items)
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
                    replyMarkup: BuildRedirectKeyboard(item.RedirectUrl),
                    cancellationToken: cancellationToken
                );
            }
        }

        private async Task EmptyCategory(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken)
        {
            var keyboard = KeyboardHelper.GetCatalogCategoriesKeyboard();

            await botClient.SendTextMessageAsync(
                message.Chat,
                "Все товары данной категории распроданы. Попробуйте другую категорию.",
                replyMarkup: keyboard,
                cancellationToken: cancellationToken
            );
        }

        private async Task NotFoundCategory(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken)
        {
            var keyboard = KeyboardHelper.GetCatalogCategoriesKeyboard();

            await botClient.SendTextMessageAsync(
                message.Chat,
                "Категория не найдена. Попробуйте  другую категорию.",
                replyMarkup: keyboard,
                cancellationToken: cancellationToken
            );
        }

        private InlineKeyboardMarkup BuildRedirectKeyboard(string url)
        {
            return new InlineKeyboardMarkup(InlineKeyboardButton.WithUrl("Заказать", url));
        }
    }
}
