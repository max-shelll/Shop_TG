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

namespace Shop_TG.BLL.Telegram.ComponentsInts.Admin
{
    public class EditCardPaymentButton
    {
        private readonly PaymentRepository _paymentRepo;

        public EditCardPaymentButton(PaymentRepository paymentRepo)
        {
            _paymentRepo = paymentRepo;
        }

        [InlineCallbackHandler<AdminBtnHeaders>(AdminBtnHeaders.EditCard)]
        public async Task Execute(ITelegramBotClient botClient, Update update)
        {
            try
            {
                string text = $"Введите данные карты";

                update.RegisterStepHandler(new StepTelegram(FillData, DateTime.Now.AddMinutes(5)));

                await Message.NotifyFromCallBack(botClient, update.CallbackQuery.Id, text);
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

                var payments = await _paymentRepo.GetById(0);

                payments.Card = message.Text;
                await _paymentRepo.Update(payments);

                update.ClearStepUserHandler();

                await Message.Send(botClient: botClient, update: update, msg: "✔ Реквизиты карты обновленные");
            }
            catch (Exception ex)
            {
                await ErrorHandler.HandleError(botClient, update, ex, "Произошла не приведенная ошибка, попробуйте через некоторое время");
            }
        }
    }
}
