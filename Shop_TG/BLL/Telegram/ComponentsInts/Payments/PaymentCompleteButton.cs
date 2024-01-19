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
using Shop_TG.DAL.Configs;
using Shop_TG.BLL.Telegram.Services;
using static System.Net.Mime.MediaTypeNames;

namespace Shop_TG.BLL.Telegram.ComponentsInts.Shop
{
    public class PaymentCompleteButton
    {
        private readonly Config _config;

        public PaymentCompleteButton(Config config)
        {
            _config = config;
        }

        [InlineCallbackHandler<ShopItemBtnHeader>(ShopItemBtnHeader.PaymentComplete)]
        public async Task Execute(ITelegramBotClient botClient, Update update)
        {
            try
            {
                var command = InlineCallback<PaymentsBtnParams>.GetCommandByCallbackOrNull(update.CallbackQuery.Data);

                await Message.NotifyFromCallBack(botClient, update.CallbackQuery.Id, "Наш менеджер скоро с вами свяжется!");

                await SendMessageToWithdrawaler(botClient, update, command.Data);
            }
            catch (Exception ex)
            {
                await ErrorHandler.HandleError(botClient, update, ex, "Произошла не приведенная ошибка, попробуйте через некоторое время");
            }
        }

        private async Task SendMessageToWithdrawaler(ITelegramBotClient botClient, Update update, PaymentsBtnParams btnParams)
        { 
            try
            {
                var callback = update.CallbackQuery;

                string text =
                $"""
                **@{callback.From.Username}** купил товар: {btnParams.ItemName}
                **Заплатил:** {btnParams.Price}
                """;

                await Message.Send(botClient: botClient, chatId: _config.Staff.SellerId, msg: text, option: new() { ParseMode = ParseMode.Markdown });
            }
            catch (Exception ex)
            {
                await ErrorHandler.HandleError(botClient, update, ex, "Произошла не приведенная ошибка, попробуйте через некоторое время");
            }
        }
    }
}
