using Telegram.Bot.Types;

namespace Sandbox.TelegramBot.Core.Commands.BotCommands
{
    public interface IBotCommand : ICommandHandler<Message>
    {
    }
}
