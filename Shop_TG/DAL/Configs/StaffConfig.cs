using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop_TG.DAL.Configs
{
    public class StaffConfig
    {
        public long OwnerId { get; set; }
        public List<long> AdminIds { get; set; }
        public long SellerId { get; set; }
        public string Support {get; set;}
    }
}
