using System.Text.Json.Serialization;
using Sandbox.TelegramBot.Core.Enums;
using Sandbox.TelegramBot.Core.Helpers;

namespace Sandbox.TelegramBot.Core.Models.CallbackDataModels
{
    internal class CategoryCallbackDataModel : ICallbackDataModel
    {
        public CallbackType Type { get; } = CallbackType.Category;

        [JsonPropertyName("C")]
        public CategoryType Category { get; init; }

        // used to deserialize
        public CategoryCallbackDataModel()
        {
        }

        public CategoryCallbackDataModel(CategoryType categoryType)
        {
            Category = categoryType;
        }

        public string Serialize() => JsonHelper.Serialize(this);
    }
}
