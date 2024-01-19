using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop_TG.DAL.Models
{
    public class ShopCategory
    {
        public string Id { get; set; } = Guid.NewGuid().ToString("N").Substring(0, 20);

        public string Name { get; set; }

        public HashSet<string> Items { get; set; }
    }
}
