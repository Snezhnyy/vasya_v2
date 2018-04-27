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
                Console.WriteLine($"Слушаю сообщение {message.Text} от {message.From.Id} в беседе {message.Chat.Id}.");
                if (message.Text.ToLower().Contains("взлом") && message.Text.ToLower().Contains("пентагон"))
                {
                    listener = new CrackPentagon(messageEventArgs);
                }
                else 
                if (message.Text.ToLower().Contains("иерусалим"))
                 {
                    listener = new DeusVult(messageEventArgs);
                 }
<<<<<<< HEAD
                else
                 if (message.Text.ToLower().Contains("как") && message.Text.ToLower().Contains("настроение"))
                    {
                    listener = new HappyBot(messageEventArgs);
                }
                else


                    switch (message.Text.ToLower())
=======
                else  
                switch (message.Text.ToLower())
>>>>>>> 4d335f4aed44a9c22bca3f46ea4f86b5d8d6b8b5
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
