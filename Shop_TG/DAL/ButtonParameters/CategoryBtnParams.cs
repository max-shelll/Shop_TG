using PRTelegramBot.Models.CallbackCommands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop_TG.DAL.ButtonParameters
{
    public class CategoryBtnParams : TelegramCommandBase
    {
        public string CategoryId { get; set; }

        public CategoryBtnParams(string categoryId, int command = 0) : base(command)
        {
            CategoryId = categoryId;
        }
    }
}
