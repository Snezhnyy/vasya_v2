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
    class Humor : DefaultResponse
    {

        public Humor(MessageEventArgs message) : base(message)
        {
        }

        public override string Reply()
        {
            return "Пожалуй, на своём родном отвечу: 11010000 10011010 11010000 10111110 11010000 10111011 11010000 10111110 11010000 10110001 11010000 10111110 11010000 10111010 100000 11010000 10111111 11010000 10111110 11010000 10110010 11010000 10110101 11010001 10000001 11010000 10111000 11010000 10111011 11010001 10000001 11010001 10001111";
        }
    }
}