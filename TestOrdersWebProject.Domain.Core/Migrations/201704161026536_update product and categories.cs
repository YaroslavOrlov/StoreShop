namespace TestOrdersWebProject.Domain.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateproductandcategories : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ProductCategoryProducts",
                c => new
                    {
                        ProductCategory_Id = c.Int(nullable: false),
                        Product_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ProductCategory_Id, t.Product_Id })
                .ForeignKey("dbo.ProductCategories", t => t.ProductCategory_Id, cascadeDelete: true)
                .ForeignKey("dbo.Products", t => t.Product_Id, cascadeDelete: true)
                .Index(t => t.ProductCategory_Id)
                .Index(t => t.Product_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ProductCategoryProducts", "Product_Id", "dbo.Products");
            DropForeignKey("dbo.ProductCategoryProducts", "ProductCategory_Id", "dbo.ProductCategories");
            DropIndex("dbo.ProductCategoryProducts", new[] { "Product_Id" });
            DropIndex("dbo.ProductCategoryProducts", new[] { "ProductCategory_Id" });
            DropTable("dbo.ProductCategoryProducts");
        }
    }
}
