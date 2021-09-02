using Sandbox.TelegramBot.Core.Enums;

namespace Sandbox.TelegramBot.Core.Models.CallbackDataModels
{
    internal interface ICallbackDataModel
    {
        public CallbackType Type { get; }

        public string Serialize();
    }
}
