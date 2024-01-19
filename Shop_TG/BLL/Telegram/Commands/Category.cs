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
    public class Category
    {
        private readonly ShopCategoryRepository _categoryRepo;

        public Category(ShopCategoryRepository categoryRepo)
        {
            _categoryRepo = categoryRepo;
        }

        [ReplyMenuHandler("Категории 🛒")]
        public async Task Execute(ITelegramBotClient botClient, Update update)
        {
            try
            {
                var categories = await _categoryRepo.GetAll();

                if (categories == null || categories.Count <= 0)
                {
                    await Message.Send(botClient: botClient, update: update, msg: "💢 В данный момент категории отсутствуют");
                    return;
                }

                var text = "🍁 **Выберите категорию:**";

                var options = new OptionMessage()
                {
                    MenuInlineKeyboardMarkup = MenuGenerator.InlineKeyboard(1, await GetCategoryBtns(categories)),
                    ParseMode = ParseMode.Markdown,
                };

                await Message.Send(botClient: botClient, update: update, msg: text);
            }
            catch (Exception ex)
            {
                await ErrorHandler.HandleError(botClient, update, ex, "Произошла не приведенная ошибка, попробуйте через некоторое время");
            }
        }

        private async Task<List<IInlineContent>> GetCategoryBtns(List<ShopCategory> categories)
        {
            var categoryBtnList = new List<IInlineContent>();

            var tasks = categories.Select(async category =>
            {
                categoryBtnList.Add(new InlineCallback(category.Name, CategoryBtnHeader.GetItems, new CategoryBtnParams(category.Id)));
            });

            await Task.WhenAll(tasks);
            return categoryBtnList;
        }
    }
}
