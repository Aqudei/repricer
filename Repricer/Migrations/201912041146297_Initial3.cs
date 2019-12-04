namespace Repricer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial3 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.MFInventoryItems", "ProductIdType", c => c.Int());
            AlterColumn("dbo.MFInventoryItems", "ItemCondition", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.MFInventoryItems", "ItemCondition", c => c.Int(nullable: false));
            AlterColumn("dbo.MFInventoryItems", "ProductIdType", c => c.Int(nullable: false));
        }
    }
}
