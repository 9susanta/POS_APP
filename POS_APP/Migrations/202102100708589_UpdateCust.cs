namespace POS_APP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateCust : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Customers", "PurchaseDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Customers", "PurchaseDate");
        }
    }
}
