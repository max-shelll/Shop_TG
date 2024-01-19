using PRTelegramBot.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop_TG.DAL.Headers
{
    [InlineCommand]
    public enum AdminBtnHeader
    {
        EditCard = 10000,
        EditCrypto,
        ShowShopItems,
        AddShopItem,
        RemoveShopItem
    }
}
