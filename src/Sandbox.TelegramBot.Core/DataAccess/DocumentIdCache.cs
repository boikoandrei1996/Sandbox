using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

namespace Sandbox.TelegramBot.Core.DataAccess
{
    public class DocumentIdCache
    {
        private readonly ILogger _logger;
        private readonly IDistributedCache _cache;

        public DocumentIdCache(
            ILogger<DocumentIdCache> logger, 
            IDistributedCache cache
        )
        {
            _logger = logger;
            _cache = cache;
        }

        public string? GetValue(string key)
        {
            var value = _cache.GetString(key);

            return value;
        }

        public async Task<string?> GetValueAsync(string key, CancellationToken cancellationToken)
        {
            var value = await _cache.GetStringAsync(key, cancellationToken);

            return value;
        }

        public async Task SetValueAsync(string key, string documentId, CancellationToken cancellationToken)
        {
            var options = new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(1) };
            var newValue = documentId;

            var existingValue = await _cache.GetStringAsync(key, cancellationToken);
            if (existingValue is not null)
            {
                await _cache.RemoveAsync(key, cancellationToken);
                _logger.LogInformation($"{nameof(DocumentIdCache.SetValueAsync)}: removed '{key}:{existingValue}'.");
            }

            await _cache.SetStringAsync(key, newValue, options, cancellationToken);
            _logger.LogInformation($"{nameof(DocumentIdCache.SetValueAsync)}: added '{key}:{newValue}'.");
        }

        public async Task RemoveValueAsync(string key, CancellationToken cancellationToken)
        {
            await _cache.RemoveAsync(key, cancellationToken);
            _logger.LogInformation($"{nameof(DocumentIdCache.RemoveValueAsync)}: removed '{key}:_'.");
        }
    }
}
