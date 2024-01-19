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
using Shop_TG.DAL.Repositories;

namespace Shop_TG.BLL.Telegram.Commands
{
    public class Start
    {
        [SlashHandler("start")]
        public async Task Execute(ITelegramBotClient botClient, Update update)
        {
            try
            {
                var message = update.Message;

                var welcomeMessage =
                $"""
                Здравствуйте, {message.From.FirstName}! На связи Indi Shop 👋

                Для взаимодействия используйте меню ниже.
                Если возникают вопросы и(или) проблемы - используйте «поддержка» 📨
                """;

                var options = new OptionMessage()
                {
                    MenuReplyKeyboardMarkup = MenuGenerator.ReplyKeyboard(1, new List<KeyboardButton>()
                    {
                       new("Каталог 🛒"),
                       new("Поддержка 📨"),
                       new("О нас 📢"),
                    }),
                };

                await Message.Send(botClient: botClient, update: update, msg: welcomeMessage, option: options);
            }
            catch (Exception ex)
            {
                await ErrorHandler.HandleError(botClient, update, ex, "Произошла не приведенная ошибка, попробуйте через некоторое время");
            }
        }
    }
}
