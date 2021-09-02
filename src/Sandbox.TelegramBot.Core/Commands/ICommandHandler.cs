using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;

namespace Sandbox.TelegramBot.Core.Commands
{
    public interface ICommandHandler<TData>
    {
        string Key { get; }

        Task Handle(ITelegramBotClient botClient, TData data, CancellationToken cancellationToken);
    }
}
