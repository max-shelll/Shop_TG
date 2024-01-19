using PRTelegramBot.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop_TG.DAL.Headers
{
    [InlineCommand]
    public enum ShopItemBtnHeader
    {
        GetDetails = 2000,
        BuyItemByCard = 2001,
        BuyItemByCrypto = 2002,
        PaymentComplete = 2003
    }
}
