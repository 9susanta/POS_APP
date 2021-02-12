namespace POS_APP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CompanyEnity1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Companies",
                c => new
                    {
                        CompanyId = c.Int(nullable: false, identity: true),
                        ShopName = c.String(),
                        ContactNo = c.String(),
                        ShopAddress = c.String(),
                        PrinterName = c.String(),
                    })
                .PrimaryKey(t => t.CompanyId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Companies");
        }
    }
}
