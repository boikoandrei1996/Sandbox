using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using Sandbox.TelegramBot.Core.Helpers;
using Sandbox.TelegramBot.Core.Models.Documents;

namespace Sandbox.TelegramBot.Core.DataAccess
{
    public class SurveyAnswerRepository
    {
        private readonly ILogger _logger;
        private readonly IMongoCollection<SurveyAnswerDocument> _collection;

        public SurveyAnswerRepository(
            ILogger<SurveyAnswerRepository> logger,
            IMongoCollection<SurveyAnswerDocument> collection
        )
        {
            _logger = logger;
            _collection = collection;
        }

        public async Task<SurveyAnswerDocument> CreateAsync(SurveyAnswerDocument document, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(document.Id))
            {
                document.Id = MongoHelper.GenerateId();
            }

            if (document.CreatedAt == default)
            {
                document.CreatedAt = DateTime.UtcNow;
            }

            await _collection.InsertOneAsync(document, cancellationToken: cancellationToken);

            return document;
        }
    }
}
