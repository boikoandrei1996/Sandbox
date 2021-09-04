using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Sandbox.TelegramBot.Core.Models
{
    public enum SurveyState
    {
        None,
        Question1,
        RequestDetailsQuestion1,
        Question2,
        RequestDetailsQuestion2,
        Question3,
        Question4,
        RequestDetailsQuestion4,
        RequestName,
        RequestEmail,
        RequestPhone
    }

    public class SurveyForm
    {
        [JsonInclude]
        [JsonPropertyName("State")]
        public SurveyState CurrentState { get; private set; }

        [JsonInclude]
        [JsonPropertyName("Answers")]
        public Dictionary<SurveyState, string> Answers { get; private set; }

        public SurveyForm()
        {
            CurrentState = SurveyState.None;
            Answers = new();
        }

        [JsonIgnore]
        public bool IsNoneState => CurrentState == SurveyState.None;

        [JsonIgnore]
        public bool IsQuestionState =>
            CurrentState == SurveyState.Question1 ||
            CurrentState == SurveyState.Question2 ||
            CurrentState == SurveyState.Question3 ||
            CurrentState == SurveyState.Question4;

        [JsonIgnore]
        public bool IsRequestInputState =>
            CurrentState == SurveyState.RequestDetailsQuestion1 ||
            CurrentState == SurveyState.RequestDetailsQuestion2 ||
            CurrentState == SurveyState.RequestDetailsQuestion4 ||
            CurrentState == SurveyState.RequestName ||
            CurrentState == SurveyState.RequestEmail ||
            CurrentState == SurveyState.RequestPhone;

        public void MoveNextState(bool isCustomAnswer = false)
        {
            var nextState = CurrentState switch
            {
                SurveyState.None => SurveyState.Question1,
                SurveyState.Question1 => isCustomAnswer ? SurveyState.RequestDetailsQuestion1 : SurveyState.Question2,
                SurveyState.RequestDetailsQuestion1 => SurveyState.Question2,
                SurveyState.Question2 => isCustomAnswer ? SurveyState.RequestDetailsQuestion2 : SurveyState.Question3,
                SurveyState.RequestDetailsQuestion2 => SurveyState.Question3,
                SurveyState.Question3 => SurveyState.Question4,
                SurveyState.Question4 => isCustomAnswer ? SurveyState.RequestDetailsQuestion4 : SurveyState.RequestName,
                SurveyState.RequestDetailsQuestion4 => SurveyState.RequestName,
                SurveyState.RequestName => SurveyState.RequestEmail,
                SurveyState.RequestEmail => SurveyState.RequestPhone,
                SurveyState.RequestPhone => SurveyState.None,
                _ => throw new NotImplementedException(nameof(SurveyState))
            };

            CurrentState = nextState;
        }

        public void AddCurrentStateAnswer(string answer)
        {
            Answers.Add(CurrentState, answer);
        }
    }
}
