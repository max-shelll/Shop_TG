using PRTelegramBot.Attributes;
using PRTelegramBot.Models;
using Shop_TG.DAL.Privileges;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot;
using Shop_TG.DAL.Headers;
using Shop_TG.DAL.Repositories;
using PRTelegramBot.Models.InlineButtons;
using Shop_TG.DAL.ButtonParameters;
using PRTelegramBot.Utils;
using Telegram.Bot.Types.Enums;
using PRTelegramBot.Interface;
using Shop_TG.DAL.Models;
using Message = PRTelegramBot.Helpers.Message;
using Shop_TG.BLL.Telegram.Handlers;

namespace Shop_TG.BLL.Telegram.ComponentsInts.Shop
{
    public class GetPaymentByCardButton
    {
        private readonly PaymentRepository _paymentsRepo;

        public GetPaymentByCardButton(PaymentRepository paymentsRepo)
        {
            _paymentsRepo = paymentsRepo;
        }

        [InlineCallbackHandler<ShopItemBtnHeader>(ShopItemBtnHeader.BuyItemByCard)]
        public async Task Execute(ITelegramBotClient botClient, Update update)
        {
            try
            {
                var command = InlineCallback<PaymentsBtnParams>.GetCommandByCallbackOrNull(update.CallbackQuery.Data);

                var payments = await _paymentsRepo.GetById(0);

                string text = $"🎯 **Реквизиты:** `{payments.Card}`\n**К оплате:** {command.Data.Price}";

                var options = new OptionMessage()
                {
                    MenuInlineKeyboardMarkup = MenuGenerator.InlineKeyboard(1, new()
                    {
                        new InlineCallback("✔ Подтвердить оплату", ShopItemBtnHeader.PaymentComplete, command.Data),
                    }),
                    ParseMode = ParseMode.Markdown,
                };

                await Message.Send(botClient: botClient, update: update, msg: text, option: options);
            }
            catch (Exception ex)
            {
                await ErrorHandler.HandleError(botClient, update, ex, "Произошла не приведенная ошибка, попробуйте через некоторое время");
            }
        }
    }
}
