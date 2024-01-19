using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop_TG.DAL.Configs
{
    public class Config
    {
        public string BotToken { get; set; }

        public MongoConfig Mongo { get; set; }
        public StaffConfig Staff { get; set; }
    }
}
