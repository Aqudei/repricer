namespace Repricer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial3 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ListedItems", "ProductIdType", c => c.Int());
            AlterColumn("dbo.ListedItems", "ItemCondition", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ListedItems", "ItemCondition", c => c.Int(nullable: false));
            AlterColumn("dbo.ListedItems", "ProductIdType", c => c.Int(nullable: false));
        }
    }
}
