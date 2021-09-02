using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using Sandbox.TelegramBot.Core.Commands;
using Sandbox.TelegramBot.Core.DataAccess;
using Sandbox.TelegramBot.Core.Models.Documents;
using Sandbox.TelegramBot.Core.Services;
using StackExchange.Redis;
using Telegram.Bot;

namespace Sandbox.TelegramBot.Core.Helpers
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDbAccess(this IServiceCollection services, BotConfigurationBase.DbSection dbSection)
        {
            var mongo = new MongoClient(dbSection.ConnectionString);
            var database = mongo.GetDatabase(dbSection.Name);

            services
                .AddSingleton(database)
                .AddSingleton(database.GetCollection<SurveyAnswerDocument>("survey_answer"))
                .AddSingleton<SurveyAnswerRepository>()
                .AddSingleton(database.GetCollection<HistoryDocument>("history"))
                .AddSingleton<HistoryRepository>();

            return services;
        }

        public static IServiceCollection AddCacheAccess(this IServiceCollection services, BotConfigurationBase.CacheSection cacheSection)
        {
            services
                .AddStackExchangeRedisCache(options =>
                {
                    options.ConfigurationOptions = new ConfigurationOptions
                    {
                        Password = cacheSection.Password
                    };
                    options.ConfigurationOptions.EndPoints.Add(cacheSection.ConnectionString);
                })
                .AddSingleton<SurveyFormCache>()
                .AddSingleton<DocumentIdCache>();

            return services;
        }

        public static IServiceCollection AddCommands(this IServiceCollection services)
        {
            services
                .Scan(scan => scan
                    .FromAssembliesOf(typeof(ICommandHandler<>))
                    .AddClasses(filter => filter.AssignableTo(typeof(ICommandHandler<>)))
                    .AsSelf()
                    .WithSingletonLifetime()
                )
                .AddSingleton<CommandFactory>();

            return services;
        }

        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services
                .AddSingleton<HandleBotUpdateService>()
                .AddSingleton<HandleSurveyStateService>();

            return services;
        }

        public static IServiceCollection AddTelegramBotClient(this IServiceCollection services, BotConfigurationBase.BotSection botSection)
        {
            services
                .AddSingleton<ITelegramBotClient>(new TelegramBotClient(botSection.Token));

            return services;
        }
    }
}
