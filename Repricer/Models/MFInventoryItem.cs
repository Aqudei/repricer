using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repricer.Models
{
    class MFInventoryItem
    {
        public int Id { get; set; }
        public string ItemName { get; set; }
        public string ItemDescription { get; set; }
        public string ListingId { get; set; }
        public string SellerSku { get; set; }
        public double Price { get; set; }
        public int? Quantity { get; set; }

        public DateTime? OpenDate { get; set; }
        public String OpenDateTimeZone { get; set; }
        public string ImageUrl { get; set; }
        public bool? ItemIsMarketPlace { get; set; }
        public int? ProductIdType { get; set; }
        public double? ZshopShippingFee { get; set; }
        public string ItemNote { get; set; }
        public int? ItemCondition { get; set; }
        public string ZshopCategory1 { get; set; }
        public string ZshopBrowsePath { get; set; }
        public string ZshopStorefrontFeature { get; set; }
        public string Asin1 { get; set; }
        public string Asin2 { get; set; }
        public string Asin3 { get; set; }
        public bool? WillShopInternationally { get; set; }
        public bool? ExpeditedShipping { get; set; }
        public string ZshopBoldFace { get; set; }
        public string ProductId { get; set; }
        public string BidForFeaturedPlacement { get; set; }
        public string AddDelete { get; set; }
        public string PendingQuantity { get; set; }
        public string FulfillmentChannel { get; set; }
        public string MerchantShoppingGroup { get; set; }

        public override string ToString()
        {
            return $"Name:{ItemName}, OpenDate: {OpenDate}, ItemIsMarketPlace: {ItemIsMarketPlace}";
        }
    }
}
