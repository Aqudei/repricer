namespace Repricer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class firstMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.FBAInventoryItems",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SellerSku = c.String(maxLength: 4000),
                        FulfillmentChannelSku = c.String(maxLength: 4000),
                        Asin = c.String(maxLength: 4000),
                        ConditionType = c.String(maxLength: 4000),
                        WarehouseConditionCode = c.String(maxLength: 4000),
                        QuantityAvailable = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ListedItems",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ItemName = c.String(maxLength: 4000),
                        ItemDescription = c.String(maxLength: 4000),
                        ListingId = c.String(maxLength: 4000),
                        SellerSku = c.String(maxLength: 4000),
                        Price = c.Double(nullable: false),
                        Quantity = c.Int(),
                        OpenDate = c.DateTime(),
                        OpenDateTimeZone = c.String(maxLength: 4000),
                        ImageUrl = c.String(maxLength: 4000),
                        ItemIsMarketPlace = c.Boolean(),
                        ProductIdType = c.Int(),
                        ZshopShippingFee = c.Double(),
                        ItemNote = c.String(maxLength: 4000),
                        ItemCondition = c.Int(),
                        ZshopCategory1 = c.String(maxLength: 4000),
                        ZshopBrowsePath = c.String(maxLength: 4000),
                        ZshopStorefrontFeature = c.String(maxLength: 4000),
                        Asin1 = c.String(maxLength: 4000),
                        Asin2 = c.String(maxLength: 4000),
                        Asin3 = c.String(maxLength: 4000),
                        WillShopInternationally = c.Boolean(),
                        ExpeditedShipping = c.Boolean(),
                        ZshopBoldFace = c.String(maxLength: 4000),
                        ProductId = c.String(maxLength: 4000),
                        BidForFeaturedPlacement = c.String(maxLength: 4000),
                        AddDelete = c.String(maxLength: 4000),
                        PendingQuantity = c.String(maxLength: 4000),
                        FulfillmentChannel = c.String(maxLength: 4000),
                        MerchantShoppingGroup = c.String(maxLength: 4000),
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
