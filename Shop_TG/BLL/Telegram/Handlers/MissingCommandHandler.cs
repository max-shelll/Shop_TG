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
    public class MissingCommandHandler
    {
        public async Task Execute(ITelegramBotClient botclient, Update update)
        {
            string text = "💢 Введённая вами команда не найдена";

            await Message.Send(botclient, update, text);
        }
    }
}
