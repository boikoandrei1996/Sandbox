using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Sandbox.TelegramBot.Webhook.HealthProbes
{
    public class RedisCacheProbe : IHealthCheck
    {
        public const string Name = "redis_cache_probe";

        public static readonly TimeSpan? Timeout = TimeSpan.FromSeconds(5);

        private readonly IDistributedCache _cache;

        public RedisCacheProbe(IDistributedCache cache)
        {
            _cache = cache;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _cache.GetAsync("probe", cancellationToken);

                return HealthCheckResult.Healthy("Success");
            }
            catch
            {
                return HealthCheckResult.Unhealthy("Failed");
            }
        }
    }
}
