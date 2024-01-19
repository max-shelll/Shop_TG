using PRTelegramBot.Attributes;
using PRTelegramBot.Models.InlineButtons;
using PRTelegramBot.Models;
using PRTelegramBot.Utils;
using Shop_TG.DAL.ButtonParameters;
using Shop_TG.DAL.Headers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types;
using Telegram.Bot;
using Message = PRTelegramBot.Helpers.Message;
using Shop_TG.BLL.Telegram.Handlers;
using Shop_TG.DAL.Repositories;
using PRTelegramBot.Extensions;

namespace Shop_TG.BLL.Telegram.ComponentsInts.Admin.ShopItems
{
    public class ShowShopItemsButton
    {
        private readonly ShopItemRepository _shopRepo;

        public ShowShopItemsButton(ShopItemRepository shopRepo)
        {
            _shopRepo = shopRepo;
        }

        [InlineCallbackHandler<AdminBtnHeader>(AdminBtnHeader.ShowShopItems)]
        public async Task Execute(ITelegramBotClient botClient, Update update)
        {
            try
            {
                var shopItems = await _shopRepo.GetAll();
                var text = new StringBuilder();

                var options = new OptionMessage()
                {
                    MenuInlineKeyboardMarkup = MenuGenerator.InlineKeyboard(2, new()
                    {
                        new InlineCallback("Создать товар", AdminBtnHeader.AddShopItem),
                        new InlineCallback("Удалить товар", AdminBtnHeader.RemoveShopItem),
                    }),
                    ParseMode = ParseMode.Markdown,
                };

                if (shopItems.Count > 0)
                {
                    var appentShopData = shopItems.Select(async item =>
                    {
                        text.AppendLine(
                        $"""
                        Id: `{item.Id}`
                        Name: `{item.Name}`
                        Description: `{item.Description}`
                        Price: `{item.Price}`

                        """);
                    });
                    await Task.WhenAll(appentShopData);
                }
                else
                {
                    text.AppendLine("❌ **Данных нету...**");
                }

                await Message.Send(botClient: botClient, update: update, msg: text.ToString(), option: options);
            }
            catch (Exception ex)
            {
                await ErrorHandler.HandleError(botClient, update, ex, "Произошла не приведенная ошибка, попробуйте через некоторое время");
            }
        }
    }
}
