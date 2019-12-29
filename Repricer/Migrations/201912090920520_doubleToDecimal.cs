namespace Repricer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class doubleToDecimal : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ListedItems", "LandedPrice", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.ListedItems", "ZshopShippingFee", c => c.Decimal(precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ListedItems", "ZshopShippingFee", c => c.Double());
            AlterColumn("dbo.ListedItems", "LandedPrice", c => c.Double(nullable: false));
        }
    }
}
