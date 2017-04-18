namespace TestOrdersWebProject.Domain.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatepurchasemodelandaddedannotations : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Purchases", "CurrencyId", c => c.Int(nullable: false));
            AddColumn("dbo.Purchases", "SummaryPrice", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.CharacteristicsGroups", "Name", c => c.String(nullable: false));
            AlterColumn("dbo.ProductCharacteristics", "Name", c => c.String(nullable: false));
            AlterColumn("dbo.ProductCharacteristics", "Value", c => c.String(nullable: false));
            AlterColumn("dbo.Products", "Name", c => c.String(nullable: false));
            AlterColumn("dbo.Products", "Description", c => c.String(nullable: false));
            AlterColumn("dbo.ProductCategories", "Name", c => c.String(nullable: false));
            AlterColumn("dbo.Currencies", "Name", c => c.String(nullable: false));
            CreateIndex("dbo.Purchases", "CurrencyId");
            AddForeignKey("dbo.Purchases", "CurrencyId", "dbo.Currencies", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Purchases", "CurrencyId", "dbo.Currencies");
            DropIndex("dbo.Purchases", new[] { "CurrencyId" });
            AlterColumn("dbo.Currencies", "Name", c => c.String());
            AlterColumn("dbo.ProductCategories", "Name", c => c.String());
            AlterColumn("dbo.Products", "Description", c => c.String());
            AlterColumn("dbo.Products", "Name", c => c.String());
            AlterColumn("dbo.ProductCharacteristics", "Value", c => c.String());
            AlterColumn("dbo.ProductCharacteristics", "Name", c => c.String());
            AlterColumn("dbo.CharacteristicsGroups", "Name", c => c.String());
            DropColumn("dbo.Purchases", "SummaryPrice");
            DropColumn("dbo.Purchases", "CurrencyId");
        }
    }
}
