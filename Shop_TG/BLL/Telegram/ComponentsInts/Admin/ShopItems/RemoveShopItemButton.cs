using PRTelegramBot.Attributes;
using PRTelegramBot.Models;
using Shop_TG.DAL.Headers;
using Shop_TG.DAL.Privileges;
using Shop_TG.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot;
using Message = PRTelegramBot.Helpers.Message;
using PRTelegramBot.Extensions;
using Shop_TG.BLL.Telegram.Handlers;
using Shop_TG.DAL.Models;

namespace Shop_TG.BLL.Telegram.ComponentsInts.Admin.ShopItems
{
    public class RemoveShopItemButton
    {
        private readonly ShopItemRepository _shopRepo;

        public RemoveShopItemButton(ShopItemRepository shopRepo)
        {
            _shopRepo = shopRepo;
        }

        [Access((int)(UserPrivilege.Admin))]
        [InlineCallbackHandler<AdminBtnHeader>(AdminBtnHeader.RemoveShopItem)]
        public async Task Execute(ITelegramBotClient botClient, Update update)
        {
            try
            {
                string text = "Введите id товара";

                update.RegisterStepHandler(new StepTelegram(FillData, DateTime.Now.AddMinutes(5)));

                await Message.NotifyFromCallBack(
                    botClient: botClient,
                    callbackQueryId: update.CallbackQuery.Id,
                    msg: text);
            }
            catch (Exception ex)
            {
                await ErrorHandler.HandleError(botClient, update, ex, "Произошла не приведенная ошибка, попробуйте через некоторое время");
            }
        }

        private async Task FillData(ITelegramBotClient botClient, Update update)
        {
            try
            {
                await _shopRepo.DeleteById(update.Message.Text);

                update.ClearStepUserHandler();

                await Message.Send(botClient: botClient, update: update, msg: "✔ Товар успешно удален");
            }
            catch (Exception ex)
            {
                await ErrorHandler.HandleError(botClient, update, ex);
            }
        }
    }
}
