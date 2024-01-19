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
    public class GetItemDetailsButton
    {
        private readonly ShopItemRepository _shopItemRepo;

        public GetItemDetailsButton(ShopItemRepository shopItemRepo)
        {
            _shopItemRepo = shopItemRepo;
        }

        [InlineCallbackHandler<ShopItemBtnHeader>(ShopItemBtnHeader.GetDetails)]
        public async Task Execute(ITelegramBotClient botClient, Update update)
        {
            try
            {
                var command = InlineCallback<ShopItemBtnParams>.GetCommandByCallbackOrNull(update.CallbackQuery.Data);

                var item = await _shopItemRepo.GetById(command.Data.ShopItemId);

                string text =
                $"""
                🎡 **Товар: {item.Name}**
                **Описание:** `{item.Description}`
                **Цена: {item.Price}**
                """;

                var options = new OptionMessage()
                {
                    MenuInlineKeyboardMarkup = MenuGenerator.InlineKeyboard(2, new()
                    {
                        new InlineCallback("Оплатить картой 💳", ShopItemBtnHeader.BuyItemByCard, new PaymentsBtnParams(item.Price, item.Name)),
                        new InlineCallback("Оплатить криптой 🍘", ShopItemBtnHeader.BuyItemByCrypto, new PaymentsBtnParams(item.Price, item.Name)),
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
