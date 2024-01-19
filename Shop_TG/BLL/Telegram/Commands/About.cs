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

namespace Shop_TG.BLL.Telegram.Commands
{
    public class About
    {
        [ReplyMenuHandler("О нас 📢")]
        public async Task Execute(ITelegramBotClient botClient, Update update)
        {
            try
            {
                var message = update.Message;

                var text =
                $"""
                СКОРО
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
