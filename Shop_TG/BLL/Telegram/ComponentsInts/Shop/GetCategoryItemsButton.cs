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
    public class GetCategoryItemsButton
    {
        private readonly ShopCategoryRepository _categoryRepo;
        private readonly ShopItemRepository _shopItemRepo;

        public GetCategoryItemsButton(ShopCategoryRepository categoryRepo, ShopItemRepository shopItemRepo)
        {
            _categoryRepo = categoryRepo;
            _shopItemRepo = shopItemRepo;
        }

        [InlineCallbackHandler<CategoryBtnHeader>(CategoryBtnHeader.GetItems)]
        public async Task Execute(ITelegramBotClient botClient, Update update)
        {
            try
            {
                var command = InlineCallback<CategoryBtnParams>.GetCommandByCallbackOrNull(update.CallbackQuery.Data);

                var category = await _categoryRepo.GetById(command.Data.CategoryId);

                string text = $"🎡 **Категория: {category.Name}**";

                var options = new OptionMessage()
                {
                    MenuInlineKeyboardMarkup = MenuGenerator.InlineKeyboard(1, await GetCategoryItemsBtns(category)),
                    ParseMode = ParseMode.Markdown,
                };

                await Message.Send(botClient: botClient, update: update, msg: text);
            }
            catch (Exception ex)
            {
                await ErrorHandler.HandleError(botClient, update, ex, "Произошла не приведенная ошибка, попробуйте через некоторое время");
            }
        }

        private async Task<List<IInlineContent>> GetCategoryItemsBtns(ShopCategory category)
        {
            var shopItemsBtnList = new List<IInlineContent>();

            var tasks = category.Items.Select(async itemId =>
            {
                var item = await _shopItemRepo.GetById(itemId);

                shopItemsBtnList.Add(new InlineCallback(item.Name, ShopItemBtnHeader.GetDetails, new ShopItemBtnParams(itemId)));
            });

            await Task.WhenAll(tasks);
            return shopItemsBtnList;
        }
    }
}
