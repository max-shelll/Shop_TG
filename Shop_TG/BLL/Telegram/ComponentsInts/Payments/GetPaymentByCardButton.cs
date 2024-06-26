﻿using PRTelegramBot.Attributes;
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
        private readonly ShopItemRepository _shopRepo;

        public GetPaymentByCardButton(PaymentRepository paymentsRepo, ShopItemRepository shopRepo)
        {
            _paymentsRepo = paymentsRepo;
            _shopRepo = shopRepo;
        }

        [InlineCallbackHandler<ShopItemBtnHeader>(ShopItemBtnHeader.BuyItemByCard)]
        public async Task Execute(ITelegramBotClient botClient, Update update)
        {
            try
            {
                var command = InlineCallback<PaymentsBtnParams>.GetCommandByCallbackOrNull(update.CallbackQuery.Data);
                var shopItem = await _shopRepo.GetById(command.Data.ItemId);

                var payments = await _paymentsRepo.GetById(0);

                string text = $"🎯 **Реквизиты:** `{payments.Card}`\n**Товар:** `{shopItem.Name}`\n**К оплате:** {shopItem.Price}";

                var options = new OptionMessage()
                {
                    MenuInlineKeyboardMarkup = MenuGenerator.InlineKeyboard(1, new()
                    {
                        new InlineCallback("✔ Подтвердить оплату", ShopItemBtnHeader.PaymentComplete, new PaymentsBtnParams(shopItem.Id)),
                    }),
                    ParseMode = ParseMode.Markdown,
                };

                await Message.SendPhoto(botClient: botClient, chatId: update.CallbackQuery.From.Id, msg: text, filePath: "Images/Catalog.jpg", option: options);
            }
            catch (Exception ex)
            {
                await ErrorHandler.HandleError(botClient, update, ex, "Произошла не приведенная ошибка, попробуйте через некоторое время");
            }
        }
    }
}
