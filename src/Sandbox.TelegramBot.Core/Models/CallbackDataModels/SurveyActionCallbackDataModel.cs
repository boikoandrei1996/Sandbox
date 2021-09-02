using Sandbox.TelegramBot.Core.Enums;
using Sandbox.TelegramBot.Core.Helpers;

namespace Sandbox.TelegramBot.Core.Models.CallbackDataModels
{
    internal class SurveyActionCallbackDataModel : ICallbackDataModel
    {
        public CallbackType Type { get; init; }

        // used to deserialize
        public SurveyActionCallbackDataModel()
        {
        }

        public SurveyActionCallbackDataModel(CallbackType callbackType)
        {
            Type = callbackType;
        }

        public string Serialize() => JsonHelper.Serialize(this);
    }
}
