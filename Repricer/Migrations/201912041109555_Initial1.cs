namespace Repricer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial1 : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.FBAInventories", newName: "FBAInventoryItems");
            RenameTable(name: "dbo.MFInventories", newName: "MFInventoryItems");
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.MFInventoryItems", newName: "MFInventories");
            RenameTable(name: "dbo.FBAInventoryItems", newName: "FBAInventories");
        }
    }
}
