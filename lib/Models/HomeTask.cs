﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Telegram.Bot.vasya_v2
{
    public class THomeTask
    {
        public int Id { get; set; }
        public long TelegramId { get; set; }
        public DateTime Date { get; set; }
        public string Subject { get; set; }
        public string Task { get; set; }
    }
}
