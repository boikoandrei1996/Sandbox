using MongoDB.Bson;

namespace Sandbox.TelegramBot.Core.Helpers
{
    internal static class MongoHelper
    {
        public static string GenerateId() => ObjectId.GenerateNewId().ToString();
    }
}
