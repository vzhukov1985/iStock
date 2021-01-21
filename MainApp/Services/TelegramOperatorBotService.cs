using DbCore;
using DbCore.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;

namespace MainApp.Services
{
    public static class TelegramOperatorBotService
    {

        private static ITelegramBotClient botClient;
        public static bool isRunning = false;

        private static DbContextOptions<MainDbContext> dbContextOptions;

        static TelegramOperatorBotService()
        {
            var optionsBuilder = new DbContextOptionsBuilder<MainDbContext>();

            dbContextOptions = optionsBuilder
                    .UseMySql(SettingsService.GetDbConnectionString())
                    .Options;
        }

        public static void StartBot()
        {
            botClient = new TelegramBotClient(SettingsService.GetTelegramOperatorBotToken());
            botClient.OnMessage += Bot_OnMessage;
            botClient.StartReceiving();
            isRunning = true;
        }

        public static void Broadcast(string str)
        {
            if (isRunning == false)
                return;

            using (MainDbContext db = new MainDbContext(dbContextOptions))
            {
                var allOps = db.TelegramUsers.Where(tu => tu.Role == "Operator");
                foreach (var op in allOps)
                {
                    botClient.SendTextMessageAsync(op.TelegramUserId, str, Telegram.Bot.Types.Enums.ParseMode.Html);
                }
            }
        }

        private static void Bot_OnMessage(object sender, MessageEventArgs e)
        {
            if (isRunning == false)
                return;

            try
            {

                if (e.Message.Text.ToUpper() == "ADDME")
                {
                    using (MainDbContext db = new MainDbContext(dbContextOptions))
                    {
                        var existingUser = db.TelegramUsers.Where(tu => tu.TelegramUserId == e.Message.Chat.Id && tu.Role == "Operator").FirstOrDefault();
                        if (existingUser == null)
                        {
                            db.Add(new TelegramUser
                            {
                                Id = Guid.NewGuid(),
                                TelegramUserId = e.Message.Chat.Id,
                                Role = "Operator"
                            });
                            db.SaveChanges();
                            botClient.SendTextMessageAsync(e.Message.Chat.Id, "Вы добавлены в список рассылки обновлений базы");
                        }
                        else
                        {
                            botClient.SendTextMessageAsync(e.Message.Chat.Id, "Вы уже есть в списке получателей рассылки обновлений базы");
                        }
                    }
                }

                if (e.Message.Text.ToUpper() == "REMOVEME")
                {
                    using (MainDbContext db = new MainDbContext(dbContextOptions))
                    {
                        var existingUser = db.TelegramUsers.Where(tu => tu.TelegramUserId == e.Message.Chat.Id && tu.Role == "Operator").FirstOrDefault();
                        if (existingUser == null)
                        {

                            botClient.SendTextMessageAsync(e.Message.Chat.Id, "Вас нет в списке получателей рассылки обновлений базы");
                        }
                        else
                        {
                            db.Remove(existingUser);
                            db.SaveChanges();
                            botClient.SendTextMessageAsync(e.Message.Chat.Id, "Вы были исключены из списка рассылки обновлений базы");
                        }
                    }
                }
            }
            catch
            {
                return;
            }
        }
    }
}
