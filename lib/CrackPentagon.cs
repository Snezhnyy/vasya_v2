using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot.Args;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InlineQueryResults;
using Telegram.Bot.Types.ReplyMarkups;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Telegram.Bot.vasya_v2
{
    class CrackPentagon : DefaultResponse
    {
        private MessageEventArgs message;

        public CrackPentagon(MessageEventArgs message) : base(message)
        {
        }

        public override string Reply()
        {
            return "Пентагон взломан!";
        }
    }
}