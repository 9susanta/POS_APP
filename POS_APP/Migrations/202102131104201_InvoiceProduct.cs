namespace POS_APP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InvoiceProduct : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.InvoiceProducts",
                c => new
                    {
                        InvoiceProductId = c.Guid(nullable: false, identity: true),
                        ProductIds = c.Int(nullable: false),
                        SellRecordsId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.InvoiceProductId)
                .ForeignKey("dbo.SellRecords", t => t.SellRecordsId, cascadeDelete: true)
                .Index(t => t.SellRecordsId);
            
            AddColumn("dbo.SellRecords", "PurchaseDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.InvoiceProducts", "SellRecordsId", "dbo.SellRecords");
            DropIndex("dbo.InvoiceProducts", new[] { "SellRecordsId" });
            DropColumn("dbo.SellRecords", "PurchaseDate");
            DropTable("dbo.InvoiceProducts");
        }
    }
}
