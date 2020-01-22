namespace Repricer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.FBAInventoryItems",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SellerSku = c.String(),
                        FulfillmentChannelSku = c.String(),
                        Asin = c.String(),
                        ConditionType = c.String(),
                        WarehouseConditionCode = c.String(),
                        QuantityAvailable = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ListedItems",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ItemName = c.String(),
                        ItemDescription = c.String(),
                        ListingId = c.String(),
                        SellerSku = c.String(),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Quantity = c.Int(),
                        OpenDate = c.DateTime(),
                        OpenDateTimeZone = c.String(),
                        ImageUrl = c.String(),
                        ItemIsMarketPlace = c.Boolean(),
                        ProductIdType = c.Int(),
                        ZshopShippingFee = c.Decimal(precision: 18, scale: 2),
                        ItemNote = c.String(),
                        ItemCondition = c.Int(),
                        ZshopCategory1 = c.String(),
                        ZshopBrowsePath = c.String(),
                        ZshopStorefrontFeature = c.String(),
                        Asin1 = c.String(),
                        Asin2 = c.String(),
                        Asin3 = c.String(),
                        WillShopInternationally = c.Boolean(),
                        ExpeditedShipping = c.Boolean(),
                        ZshopBoldFace = c.String(),
                        ProductId = c.String(),
                        BidForFeaturedPlacement = c.String(),
                        AddDelete = c.String(),
                        PendingQuantity = c.String(),
                        FulfillmentChannel = c.String(),
                        MerchantShoppingGroup = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.ListedItems");
            DropTable("dbo.FBAInventoryItems");
        }
    }
}
