using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repricer.Models
{
    class FBAInventoryItem
    {
        public int Id { get; set; }
        public string SellerSku { get; set; }
        public string FulfillmentChannelSku { get; set; }
        public string Asin { get; set; }
        public string ConditionType { get; set; }
        public string WarehouseConditionCode { get; set; }
        public int? QuantityAvailable { get; set; }
    }
}
