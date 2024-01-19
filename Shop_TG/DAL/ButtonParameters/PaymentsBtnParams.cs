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
        public int Price { get; set; }
        public string ItemName { get; set; }

        public PaymentsBtnParams(int price, string itemName, int command = 0) : base(command)
        {
            Price = price;
            ItemName = itemName;
        }
    }
}
