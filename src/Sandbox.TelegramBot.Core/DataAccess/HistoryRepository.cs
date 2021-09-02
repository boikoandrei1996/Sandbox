using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using Sandbox.TelegramBot.Core.Helpers;
using Sandbox.TelegramBot.Core.Models.Documents;

namespace Sandbox.TelegramBot.Core.DataAccess
{
    public class HistoryRepository
    {
        private static readonly FindOneAndUpdateOptions<HistoryDocument> Options = new()
        {
            IsUpsert = true,
            ReturnDocument = ReturnDocument.After
        };

        private readonly ILogger _logger;
        private readonly IMongoCollection<HistoryDocument> _collection;

        public HistoryRepository(
            ILogger<HistoryRepository> logger,
            IMongoCollection<HistoryDocument> collection
        )
        {
            _logger = logger;
            _collection = collection;
        }

        public async Task<HistoryDocument> PushAsync(string chatId, HistoryAction action, CancellationToken cancellationToken)
        {
            var filterBuilder = Builders<HistoryDocument>.Filter;
            var filter = filterBuilder.Eq(d => d.ChatId, chatId);

            var updateBuilder = Builders<HistoryDocument>.Update;
            var update = updateBuilder.Combine(
                updateBuilder.SetOnInsert(d => d.Id, MongoHelper.GenerateId()),
                updateBuilder.SetOnInsert(d => d.CreatedAt, action.ActionedAt),
                updateBuilder.Push(d => d.Actions, action)
            );

            var document = await _collection.FindOneAndUpdateAsync(
                filter,
                update,
                HistoryRepository.Options,
                cancellationToken
            );

            return document;
        }
    }
}
