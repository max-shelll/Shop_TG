using PRTelegramBot.Models.CallbackCommands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop_TG.DAL.ButtonParameters
{
    public class ShopItemBtnParams : TelegramCommandBase
    {
        public string ShopItemId { get; set; }

        public ShopItemBtnParams(string shopItemId, int command = 0) : base(command)
        {
            ShopItemId = shopItemId;
        }
    }
}
