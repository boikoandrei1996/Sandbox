using System;
using MongoDB.Bson.Serialization.Attributes;

namespace Sandbox.TelegramBot.Core.Models.Documents
{
    [BsonIgnoreExtraElements(Inherited = true)]
    public abstract class BaseDocument
    {
        public string? Id { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
