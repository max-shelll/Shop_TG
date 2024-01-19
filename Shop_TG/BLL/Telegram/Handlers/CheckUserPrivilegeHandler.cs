using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot;
using Message = PRTelegramBot.Helpers.Message;
using Shop_TG.DAL.Privileges;
using PRTelegramBot.Extensions;

namespace Shop_TG.BLL.Telegram.Handlers
{
    public class CheckUserPrivilegeHandler()
    {
        public async Task Execute(ITelegramBotClient botClient, Update update, Func<ITelegramBotClient, Update, Task> callback, int? flags = null)
        {
            if (flags != null)
            {
                var requreAccess = (UserPrivilege)flags.Value;
                var myAccess = GetFlagPrivilege(botClient: botClient, update: update);

                if (requreAccess.HasFlag(myAccess))
                {
                    await callback(botClient, update);
                    return;
                }

                await Message.Send(botClient: botClient, update: update, msg: $"💢 Данная команда преднозначенна только для администраторов");
                return;
            }
            string msg = "Проверка привилегий";
            await Message.Send(botClient, update, msg);
        }

        private static UserPrivilege GetFlagPrivilege(ITelegramBotClient botClient, Update update)
        {
            if (botClient.IsAdmin(update))
                return UserPrivilege.Admin;
            else
                return UserPrivilege.Guest;
        }
    }
}
