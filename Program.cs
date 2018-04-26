// Hello, Vasya

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
    class Program
    {
        private static TelegramBotClient Bot;
        static void Main(string[] args)
        {
            Bot = new TelegramBotClient(ReadToken());
            var me = Bot.GetMeAsync().Result;
            Console.Title = me.Username;
            Bot.OnMessage += BotOnMessageReceived;

            Bot.StartReceiving(Array.Empty<UpdateType>());
            Console.WriteLine($"Start listening for @{me.Username}");
            Console.WriteLine("Press enter to exit.");
            Console.ReadLine();
            Bot.StopReceiving();
        }

        private static async void BotOnMessageReceived(object sender, MessageEventArgs messageEventArgs)
        {
            var message = messageEventArgs.Message;
            if (message == null || message.Type != MessageType.TextMessage) return;
            DefaultResponse listener;
            if (message.Text.ToLower().Contains("вася"))
            {
                switch (message.Text.ToLower())
                {
                    default:
                        listener = new DefaultResponse(messageEventArgs);
                        break;     
                }  
                await Bot.SendTextMessageAsync(
                            message.Chat.Id,
                            listener.Reply(),
                            replyMarkup: new ReplyKeyboardRemove());
            }
        }

        private static string ReadToken()
        {
            return JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText("bot_passport.json"))["token"];
        }
    }
}
