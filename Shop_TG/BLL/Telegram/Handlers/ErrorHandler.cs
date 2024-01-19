using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot;
using Message = PRTelegramBot.Helpers.Message;

namespace Shop_TG.BLL.Telegram.Handlers
{
    public static class ErrorHandler
    {
        public async static Task HandleError(ITelegramBotClient botClient, Update update, Exception ex, 
            string errorMessage = "Произошла не приведенная ошибка, попробуйте через некоторое время")
        {
            await Message.Send(botClient: botClient, update: update, msg: $"💢 {errorMessage}\n{ex.Message}");
        }
    }
}
