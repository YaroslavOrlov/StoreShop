namespace TestOrdersWebProject.Domain.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addcollectionproducts : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ProductCharacteristics", "Product_Id", "dbo.Products");
            DropIndex("dbo.ProductCharacteristics", new[] { "Product_Id" });
            CreateTable(
                "dbo.ProductProductCharacteristics",
                c => new
                    {
                        Product_Id = c.Int(nullable: false),
                        ProductCharacteristic_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Product_Id, t.ProductCharacteristic_Id })
                .ForeignKey("dbo.Products", t => t.Product_Id, cascadeDelete: true)
                .ForeignKey("dbo.ProductCharacteristics", t => t.ProductCharacteristic_Id, cascadeDelete: true)
                .Index(t => t.Product_Id)
                .Index(t => t.ProductCharacteristic_Id);
            
            DropColumn("dbo.ProductCharacteristics", "Product_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ProductCharacteristics", "Product_Id", c => c.Int());
            DropForeignKey("dbo.ProductProductCharacteristics", "ProductCharacteristic_Id", "dbo.ProductCharacteristics");
            DropForeignKey("dbo.ProductProductCharacteristics", "Product_Id", "dbo.Products");
            DropIndex("dbo.ProductProductCharacteristics", new[] { "ProductCharacteristic_Id" });
            DropIndex("dbo.ProductProductCharacteristics", new[] { "Product_Id" });
            DropTable("dbo.ProductProductCharacteristics");
            CreateIndex("dbo.ProductCharacteristics", "Product_Id");
            AddForeignKey("dbo.ProductCharacteristics", "Product_Id", "dbo.Products", "Id");
        }
    }
}
