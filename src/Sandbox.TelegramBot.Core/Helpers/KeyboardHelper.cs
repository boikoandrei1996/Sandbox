using System.Linq;
using Sandbox.TelegramBot.Core.Models.CallbackDataModels;
using Telegram.Bot.Types.ReplyMarkups;

namespace Sandbox.TelegramBot.Core.Helpers
{
    public static class KeyboardHelper
    {
        public static InlineKeyboardMarkup GetCatalogCategoriesKeyboard()
        {
            var buttons = DataHelper.CatalogCategories
                .Select(keyValue =>
                    InlineKeyboardButton.WithCallbackData(
                        keyValue.Value.Title,
                        new CategoryCallbackDataModel(keyValue.Key).Serialize()
                    )
                )
                .ToArray();

            return new InlineKeyboardMarkup(buttons);
        }
    }
}
