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
    public class AddShopItemButton
    {
        private readonly ShopItemRepository _shopRepo;

        public AddShopItemButton(ShopItemRepository shopRepo)
        {
            _shopRepo = shopRepo;
        }

        [Access((int)(UserPrivilege.Admin))]
        [InlineCallbackHandler<AdminBtnHeader>(AdminBtnHeader.AddShopItem)]
        public async Task Execute(ITelegramBotClient botClient, Update update)
        {
            try
            {
                string text =
                $"""
                Введите данные для товара:
                1. Название
                2. Описание
                3. Цену

                Примечание: каждое значение должно быть с новой строки!
                """;

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
                var message = update.Message;
                var text = message.Text.Split('\n', StringSplitOptions.RemoveEmptyEntries);

                if (text.Length < 3)
                {
                    await Message.Send(botClient: botClient, update: update, msg: "💢 Недостаточно данных для создания товара.");
                    return;
                }

                var name = text[0];
                var description = text[1];
                var price = int.Parse(text[2]);

                var shopItem = new ShopItem()
                {
                    Name = name,
                    Description = description,
                    Price = price,
                };

                await _shopRepo.Create(shopItem);

                update.ClearStepUserHandler();

                await Message.Send(botClient: botClient, update: update, msg: "✔ Товар успешно создан");
            }
            catch (Exception ex)
            {
                await ErrorHandler.HandleError(botClient, update, ex, "Произошла не приведенная ошибка, попробуйте через некоторое время");
            }
        }
    }
}
