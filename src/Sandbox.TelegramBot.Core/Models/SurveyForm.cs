using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Sandbox.TelegramBot.Core.Models
{
    public enum SurveyState
    {
        None,
        Question1,
        Question2,
        RequestDetailsQuestion1,
        RequestDetailsQuestion2,
        RequestName,
        RequestEmail
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
            CurrentState == SurveyState.Question2;

        [JsonIgnore]
        public bool IsRequestInputState =>
            CurrentState == SurveyState.RequestDetailsQuestion1 ||
            CurrentState == SurveyState.RequestDetailsQuestion2 ||
            CurrentState == SurveyState.RequestName ||
            CurrentState == SurveyState.RequestEmail;

        public void MoveNextState(bool isCustomAnswer = false)
        {
            var nextState = CurrentState switch
            {
                SurveyState.None => SurveyState.Question1,
                SurveyState.Question1 => isCustomAnswer ? SurveyState.RequestDetailsQuestion1 : SurveyState.Question2,
                SurveyState.RequestDetailsQuestion1 => SurveyState.Question2,
                SurveyState.Question2 => isCustomAnswer ? SurveyState.RequestDetailsQuestion2 : SurveyState.RequestName,
                SurveyState.RequestDetailsQuestion2 => SurveyState.RequestName,
                SurveyState.RequestName => SurveyState.RequestEmail,
                SurveyState.RequestEmail => SurveyState.None,
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
