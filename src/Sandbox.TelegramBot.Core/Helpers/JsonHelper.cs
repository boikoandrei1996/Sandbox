using System.Text.Json;

namespace Sandbox.TelegramBot.Core.Helpers
{
    internal static class JsonHelper
    {
        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            IgnoreNullValues = true
        };

        public static string Serialize<TValue>(TValue value)
        {
            return JsonSerializer.Serialize(value, JsonOptions);
        }

        public static TValue Deserialize<TValue>(string json)
        {
            var result = JsonSerializer.Deserialize<TValue>(json, JsonOptions);

            return result ?? throw new JsonException("Deserialization failed");
        }

        public static TValue DeserializeAnonymousType<TValue>(string json, TValue _)
        {
            var result = JsonSerializer.Deserialize<TValue>(json, JsonOptions);

            return result ?? throw new JsonException("Deserialization failed");
        }
    }
}
