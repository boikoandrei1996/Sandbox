using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using MongoDB.Driver;

namespace Sandbox.TelegramBot.Webhook.HealthProbes
{
    public class MongoDbProbe : IHealthCheck
    {
        public const string Name = "mongo_db_probe";

        public static readonly TimeSpan? Timeout = TimeSpan.FromSeconds(5);

        private readonly IMongoDatabase _db;

        public MongoDbProbe(IMongoDatabase db)
        {
            _db = db;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken)
        {
            try
            {
                var cursor = await _db.ListCollectionNamesAsync(cancellationToken: cancellationToken);
                var result = await cursor.FirstOrDefaultAsync(cancellationToken);

                return result is null ?
                    HealthCheckResult.Degraded("Collection not found") :
                    HealthCheckResult.Healthy("Success");
            }
            catch
            {
                return HealthCheckResult.Unhealthy("Failed");
            }
        }
    }
}
