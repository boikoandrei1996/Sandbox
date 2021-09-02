using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Sandbox.TelegramBot.Core.Helpers;
using Sandbox.TelegramBot.Core.Models;
using Telegram.Bot.Types;

namespace Sandbox.TelegramBot.Core.DataAccess
{
    public class SurveyFormCache
    {
        private readonly ILogger _logger;
        private readonly IDistributedCache _cache;

        public SurveyFormCache(
            ILogger<SurveyFormCache> logger, 
            IDistributedCache cache
        )
        {
            _logger = logger;
            _cache = cache;
        }

        public SurveyForm? GetValue(Chat chat)
        {
            var key = chat.Id.ToString();

            var value = _cache.GetString(key);

            return value is null ? null : JsonHelper.Deserialize<SurveyForm>(value);
        }

        public async Task<SurveyForm?> GetValueAsync(Chat chat, CancellationToken cancellationToken)
        {
            var key = chat.Id.ToString();

            var value = await _cache.GetStringAsync(key, cancellationToken);

            return value is null ? null : JsonHelper.Deserialize<SurveyForm>(value);
        }

        public async Task SetValueAsync(Chat chat, SurveyForm surveyForm, CancellationToken cancellationToken)
        {
            var key = chat.Id.ToString();

            var options = new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(1) };
            var newValue = JsonHelper.Serialize(surveyForm);

            var existingValue = await _cache.GetStringAsync(key, cancellationToken);
            if (existingValue is not null)
            {
                await _cache.RemoveAsync(key, cancellationToken);
                _logger.LogInformation($"{nameof(SurveyFormCache.SetValueAsync)}: removed '{key}:{existingValue}'.");
            }

            await _cache.SetStringAsync(key, newValue, options, cancellationToken);
            _logger.LogInformation($"{nameof(SurveyFormCache.SetValueAsync)}: added '{key}:{newValue}'.");
        }

        public async Task RemoveValueAsync(Chat chat, CancellationToken cancellationToken)
        {
            var key = chat.Id.ToString();

            await _cache.RemoveAsync(key, cancellationToken);
            _logger.LogInformation($"{nameof(SurveyFormCache.RemoveValueAsync)}: removed '{key}:_'.");
        }
    }
}
