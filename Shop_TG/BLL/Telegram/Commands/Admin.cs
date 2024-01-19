using PRTelegramBot.Attributes;
using PRTelegramBot.Models;
using PRTelegramBot.Utils;
using Shop_TG.DAL.Privileges;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types;
using Telegram.Bot;
using PRTelegramBot.Models.InlineButtons;
using Message = PRTelegramBot.Helpers.Message;
using Shop_TG.BLL.Telegram.Handlers;
using Shop_TG.DAL.Repositories;
using Shop_TG.DAL.Headers;

namespace Shop_TG.BLL.Telegram.Commands
{
    public class Admin
    {
        private readonly PaymentRepository _paymentRepo;

        public Admin(PaymentRepository paymentRepo)
        {
            _paymentRepo = paymentRepo;
        }

        [Access((int)(UserPrivilege.Admin))]
        [SlashHandler("admin")]
        public async Task Execute(ITelegramBotClient botClient, Update update)
        {
            try
            {
                var payments = await _paymentRepo.GetById(0);

                string text =
                $"""
                Добро пожаловать в админ меню 💠

                **Карта:** `{payments.Card}`
                **Крипто:** `{payments.Crypto}`
                """;

                var options = new OptionMessage()
                {
                    MenuInlineKeyboardMarkup = MenuGenerator.InlineKeyboard(3, new()
                    {
                        new InlineCallback("Товары", AdminBtnHeader.ShowShopItems),
                        new InlineCallback("Карта", AdminBtnHeader.EditCard),
                        new InlineCallback("Крипто-кошелек", AdminBtnHeader.EditCrypto),
                    }),
                    ParseMode = ParseMode.Markdown,
                };

                await Message.Send(botClient, update, text, options);
            }
            catch (Exception ex)
            {
                await ErrorHandler.HandleError(botClient, update, ex, "Произошла не приведенная ошибка, попробуйте через некоторое время");
            }
        }
    }
}
