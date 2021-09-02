using System.Collections.Generic;

namespace Sandbox.TelegramBot.Core.Models.Documents
{
    public class SurveyAnswerDocument : BaseDocument
    {
        public Dictionary<string, string> Answers { get; init; } = new();

        public static SurveyAnswerDocument From(SurveyForm surveyForm)
        {
            var result = new SurveyAnswerDocument
            {
                Answers = new Dictionary<string, string>(surveyForm.Answers.Count)
            };

            foreach (var (key, answer) in surveyForm.Answers)
            {
                if (key == SurveyState.Question1)
                {
                    if (surveyForm.Answers.TryGetValue(SurveyState.RequestDetailsQuestion1, out var answerDetails))
                    {
                        result.Answers.Add(key.ToString(), $"{answer} ({answerDetails})");
                    }
                    else
                    {
                        result.Answers.Add(key.ToString(), answer);
                    }
                }
                else if (key == SurveyState.Question2)
                {
                    if (surveyForm.Answers.TryGetValue(SurveyState.RequestDetailsQuestion2, out var answerDetails))
                    {
                        result.Answers.Add(key.ToString(), $"{answer} ({answerDetails})");
                    }
                    else
                    {
                        result.Answers.Add(key.ToString(), answer);
                    }
                }
                else if (key == SurveyState.RequestName || key == SurveyState.RequestEmail)
                {
                    result.Answers.Add(key.ToString(), answer);
                }
            }

            return result;
        }
    }
}
