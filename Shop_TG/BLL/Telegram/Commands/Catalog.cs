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
using PRTelegramBot.Models.InlineButtons;
using Telegram.Bot.Types.Enums;
using Shop_TG.DAL.Models;
using Shop_TG.DAL.Headers;
using Shop_TG.DAL.ButtonParameters;
using PRTelegramBot.Interface;

namespace Shop_TG.BLL.Telegram.Commands
{
    public class Catalog
    {
        private readonly ShopItemRepository _shopRepo;

        public Catalog(ShopItemRepository shopRepo)
        {
            _shopRepo = shopRepo;
        }

        [ReplyMenuHandler("Каталог 🛒")]
        public async Task Execute(ITelegramBotClient botClient, Update update)
        {
            try
            {
                var shopItems = await _shopRepo.GetAll();

                if (shopItems == null || shopItems.Count <= 0)
                {
                    await Message.Send(botClient: botClient, update: update, msg: "💢 В данный момент товары отсутствуют");
                    return;
                }

                var text = "🍁 **Выберите товар:**";

                var options = new OptionMessage()
                {
                    MenuInlineKeyboardMarkup = MenuGenerator.InlineKeyboard(1, await GetShopItemBtns(shopItems)),
                    ParseMode = ParseMode.Markdown,
                };

                await Message.Send(botClient: botClient, update: update, msg: text, option: options);
            }
            catch (Exception ex)
            {
                await ErrorHandler.HandleError(botClient, update, ex, "Произошла не приведенная ошибка, попробуйте через некоторое время");
            }
        }

        private async Task<List<IInlineContent>> GetShopItemBtns(List<ShopItem> shopItems)
        { 
            var buttonList = new List<IInlineContent>();

            var tasks = shopItems.Select(async item =>
            {
                buttonList.Add(new InlineCallback(item.Name, ShopItemBtnHeader.GetDetails, new ShopItemBtnParams(item.Id)));
            });

            await Task.WhenAll(tasks);
            return buttonList;
        }
    }
}
