using System.Collections.Generic;

namespace Sandbox.TelegramBot.Core.Models
{
    internal class SurveyQuestionModel
    {
        public string Text { get; init; } = null!;
        public Dictionary<int, SurveyAnswerModel> Answers { get; init; } = null!;
    }

    internal class SurveyAnswerModel
    {
        public string Text { get; init; } = null!;
    }
}
