using Newtonsoft.Json;
using PRTelegramBot.Models.CallbackCommands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop_TG.DAL.ButtonParameters
{
    public class PaymentsBtnParams : TelegramCommandBase
    {
        public string ItemId { get; set; }

        public PaymentsBtnParams(string itemId, int command = 0) : base(command)
        {
            ItemId = itemId;
        }
    }
}
