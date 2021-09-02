namespace Sandbox.TelegramBot.Core.Models
{
    public class BotCommandInfo
    {
        /// <summary>
        /// Text of the command, 1-32 characters.
        /// Can contain only lowercase English letters, digits and underscores.
        /// </summary>
        public string Command { get; init; } = null!;

        /// <summary>
        /// Text of the alternative command.
        /// </summary>
        public string AltCommand { get; init; } = null!;

        /// <summary>
        /// Description of the command, 3-256 characters.
        /// </summary>
        public string Description { get; init; } = null!;
    }
}
