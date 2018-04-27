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
            Console.WriteLine("Migrate subjects?(y/n)");
            if (Console.ReadLine() == "y")
            {
                using (VasyaContext db = new VasyaContext())
                {
                    TSubject sub1 = new TSubject { Name = "ман" };
                    TSubject sub2 = new TSubject { Name = "гиа"};
                    TSubject sub3 = new TSubject { Name = "англ"};
                    db.Subjects.AddRange(sub1, sub2, sub3);
                    db.SaveChanges();
                }
            }

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
                var dialogs = db.Dialogs.Select(p => p.TelegramId == message.Chat.Id).ToList();
                if (dialogs.Count == 0)
                {
                    db.Dialogs.Add(new TDialog {TelegramId = message.Chat.Id} );
                    db.SaveChanges();
                    Console.WriteLine($"New dialog was added with ChatId = {message.Chat.Id}");
                }

                db.Messages.Add(new TMessage { TelegramId = message.Chat.Id, Text = message.Text.ToLower() });
                db.SaveChanges();
                Console.WriteLine($"New message was added to this dialog = {message.Text}");
                // answer to hometask
                if ((message.Text.ToLower().Contains("что") || message.Text.ToLower().Contains("какое")) && (message.Text.ToLower().Contains("дом") || message.Text.ToLower().Contains("задан") || message.Text.ToLower().Contains("задав")))
                {
                    Console.WriteLine($"Trying to send hometask...");
                    string subName = "";
                    var subjects = db.Subjects.ToList();
                    foreach (TSubject sub in subjects)
                    {
                        if (message.Text.ToLower().Contains(sub.Name))
                        {
                            subName = sub.Name;
                            break;
                        }
                    }
                    if (subName != "")
                    {
                        Console.WriteLine($"I think that the subject is {subName}");
                        var hometasks = db.HomeTasks.Where(p => (p.TelegramId == message.Chat.Id) && (p.Subject == subName)).ToList();
                        if (hometasks.Count > 0)
                        {
                            Console.WriteLine($"I have found something interesting. Sending the tasks...");
                            await Bot.SendTextMessageAsync(
                                message.Chat.Id,
                                hometasks[hometasks.Count - 1].Task,
                                replyMarkup: new ReplyKeyboardRemove());
                            return;
                        }
                        Console.WriteLine("Nothing to send.");
                    }
                }
                // add new hometask
                var dbmes = db.Messages.Where(p => p.TelegramId == message.Chat.Id).ToList();
                if (dbmes.Count > 1)
                    if ((dbmes[dbmes.Count - 2].Text.Contains("что") || dbmes[dbmes.Count - 2].Text.Contains("какое")) && (dbmes[dbmes.Count - 2].Text.Contains("дом") || dbmes[dbmes.Count - 2].Text.Contains("задан") || dbmes[dbmes.Count - 2].Text.Contains("задав")))
                    {
                        Console.WriteLine("I think that it is a hometask.");
                        var subjects = db.Subjects.ToList();
                        foreach (TSubject sub in subjects)
                        {
                            if (dbmes[dbmes.Count - 2].Text.Contains(sub.Name))
                                if (!((message.Text.ToLower().Contains("что") || message.Text.ToLower().Contains("какое")) && (message.Text.ToLower().Contains("дом") || message.Text.ToLower().Contains("задан") || message.Text.ToLower().Contains("задав"))))
                                    if (!(message.Text.ToLower().Contains("спасибо") || message.Text.ToLower().Contains("спc")))
                                        if ((message.Text.ToLower().Contains("упр") || message.Text.ToLower().Contains("задан") || message.Text.ToLower().Contains("задав") || message.Text.ToLower().Contains("номер")))
                                        {
                                            Console.WriteLine("Yes, it is. I will remember it.");
                                            db.HomeTasks.Add(new THomeTask { TelegramId = message.Chat.Id, Date = DateTime.Today, Subject = sub.Name, Task = message.Text});
                                            db.SaveChanges();
                                            return;
                                        }
                        }
                        Console.WriteLine("No, it isn't.");
                    }
            }

            if (message == null || message.Type != MessageType.TextMessage) return;
            if (message.Text.ToLower().Contains("вася"))
            {
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
        }

        private static string ReadToken()
        {
            return JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText("bot_passport.json"))["token"];
        }
    }
}
