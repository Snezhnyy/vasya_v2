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

            Console.WriteLine($"Слушаю сообщение {message.Text} от пользователя {message.From.Id} в чате {message.Chat.Id}");

            using (VasyaContext db = new VasyaContext())
            {
                var dialog = db.Dialogs.Select(p => p.TelegramId == message.Chat.Id).ToList();
                if (dialog.Count == 0)
                {
                    db.Dialogs.Add(new TDialog {TelegramId = message.Chat.Id} );
                    db.SaveChanges();
                    Console.WriteLine($"New dialog added with ChatId = {message.Chat.Id}");
                }
            }
            
            if (message == null || message.Type != MessageType.TextMessage) return;
            DefaultResponse listener;
            if (message.Text.ToLower().Contains("взлом") && message.Text.ToLower().Contains("пентагон"))
            {
                listener = new CrackPentagon(messageEventArgs);
            }
            else
            if (message.Text.ToLower().Contains("иерусалим"))
            {
                listener = new DeusVult(messageEventArgs);
            }
            else
            if (message.Text.ToLower().Contains("как") && message.Text.ToLower().Contains("настроение"))
            {
                listener = new HappyBot(messageEventArgs);
            }
            else
            if (message.Text.ToLower().Contains("анекдот"))
            {
                listener = new Humor(messageEventArgs);
            }
            else 
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

        private static string ReadToken()
        {
            return JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText("bot_passport.json"))["token"];
        }
    }
}
