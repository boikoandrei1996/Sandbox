using System;
using System.Collections.Generic;

namespace Sandbox.TelegramBot.Core.Models.Documents
{
    public class HistoryDocument : BaseDocument
    {
        public string ChatId { get; set; } = null!;
        public ICollection<HistoryAction> Actions { get; set; } = new List<HistoryAction>();
    }

    public class HistoryAction
    {
        public string Action { get; set; } = null!;
        public string Data { get; set; } = null!;
        public DateTime ActionedAt { get; set; }
    }
}
