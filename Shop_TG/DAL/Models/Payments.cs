using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop_TG.DAL.Models
{
    public class Payments
    {
        public int Id { get; set; } = 0;

        public string Card { get; set; }
        public string Crypto { get; set; }
    }
}
