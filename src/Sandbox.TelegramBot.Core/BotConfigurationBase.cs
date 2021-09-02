using System;

namespace Sandbox.TelegramBot.Core
{
    public abstract class BotConfigurationBase
    {
        public BotSection Bot { get; init; } = null!;
        public DbSection Db { get; init; } = null!;
        public CacheSection Cache { get; init; } = null!;

        public void Validate()
        {
            ValidateBotSection();
            ValidateDbSection();
            ValidateCacheSection();
        }

        protected virtual void ValidateBotSection()
        {
            if (Bot is null)
            {
                throw new InvalidOperationException($"Section {nameof(Bot)} is empty");
            }

            if (string.IsNullOrEmpty(Bot.Token))
            {
                throw new InvalidOperationException($"{nameof(Bot)}:{nameof(Bot.Token)} is empty");
            }
        }

        protected virtual void ValidateDbSection()
        {
            if (Db is null)
            {
                throw new InvalidOperationException($"Section {nameof(Db)} is empty");
            }

            if (string.IsNullOrEmpty(Db.ConnectionString))
            {
                throw new InvalidOperationException($"{nameof(Db)}:{nameof(Db.ConnectionString)} is empty");
            }

            if (string.IsNullOrEmpty(Db.Name))
            {
                throw new InvalidOperationException($"{nameof(Db)}:{nameof(Db.Name)} is empty");
            }
        }

        protected virtual void ValidateCacheSection()
        {
            if (Cache is null)
            {
                throw new InvalidOperationException($"Section {nameof(Cache)} is empty");
            }

            if (string.IsNullOrEmpty(Cache.ConnectionString))
            {
                throw new InvalidOperationException($"{nameof(Cache)}:{nameof(Cache.ConnectionString)} is empty");
            }

            if (string.IsNullOrEmpty(Cache.Password))
            {
                throw new InvalidOperationException($"{nameof(Cache)}:{nameof(Cache.Password)} is empty");
            }
        }

        public class BotSection
        {
            public string Token { get; init; } = null!;
            public string? WebhookBaseAddress { get; init; }
        }

        public class DbSection
        {
            public string ConnectionString { get; init; } = null!;
            public string Name { get; init; } = null!;
        }

        public class CacheSection
        {
            public string ConnectionString { get; init; } = null!;
            public string Password { get; init; } = null!;
        }
    }
}
