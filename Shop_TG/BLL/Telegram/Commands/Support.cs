using PRTelegramBot.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot;
using Message = PRTelegramBot.Helpers.Message;
using Shop_TG.BLL.Telegram.Handlers;
using PRTelegramBot.Models;
using PRTelegramBot.Utils;
using Telegram.Bot.Types.ReplyMarkups;
using Shop_TG.DAL.Configs;

namespace Shop_TG.BLL.Telegram.Commands
{
    public class Support
    {
        private readonly Config _config;

        public Support(Config config)
        {
            _config = config;
        }

        [ReplyMenuHandler("Поддержка 📨")]
        public async Task Execute(ITelegramBotClient botClient, Update update)
        {
            try
            {
                var text =
                $"""
                Для того что бы задать вопрос или получить помощь, обратитесь к нашему менеджеру - @{_config.Staff.Support}
                Рабочее время: 11:00 - 23:00 (мск).
                """;

                await Message.Send(botClient: botClient, update: update, msg: text);
            }
            catch (Exception ex)
            {
                await ErrorHandler.HandleError(botClient, update, ex, "Произошла не приведенная ошибка, попробуйте через некоторое время");
            }
        }
    }
}
