using System.Text.Json.Serialization;
using Sandbox.TelegramBot.Core.Enums;
using Sandbox.TelegramBot.Core.Helpers;

namespace Sandbox.TelegramBot.Core.Models.CallbackDataModels
{
    internal class SurveyQuestionCallbackDataModel : ICallbackDataModel
    {
        public CallbackType Type { get; } = CallbackType.SurveyQuestion;

        [JsonPropertyName("Q")]
        public SurveyQuestionType Question { get; init; }

        [JsonPropertyName("A")]
        public int AnswerId { get; init; }

        // used to deserialize
        public SurveyQuestionCallbackDataModel()
        {
        }

        public SurveyQuestionCallbackDataModel(SurveyQuestionType questionType, int answerId)
        {
            Question = questionType;
            AnswerId = answerId;
        }

        public string Serialize() => JsonHelper.Serialize(this);
    }
}
