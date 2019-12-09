using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repricer.Models
{
    class InventoryItem
    {
        public string Sku { get; set; }
        public string Title { get; set; }
        public int Age { get; set; }
        public int Rank { get; set; }
        public decimal CurrentPrice { get; set; }
    }
}
