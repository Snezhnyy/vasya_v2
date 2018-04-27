using System;
using System.Collections.Generic;
using System.Text;

namespace Telegram.Bot.vasya_v2
{
    public class TMessage
    {
        public int Id { get; set; }

        public long TelegramId { get; set; }
        public string Text { get; set; }
    }
}
