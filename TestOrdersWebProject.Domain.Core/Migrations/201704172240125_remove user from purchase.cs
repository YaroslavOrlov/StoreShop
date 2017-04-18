namespace TestOrdersWebProject.Domain.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removeuserfrompurchase : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Purchases", "ApplicationUserId", "dbo.AspNetUsers");
            DropIndex("dbo.Purchases", new[] { "ApplicationUserId" });
            DropColumn("dbo.Purchases", "ApplicationUserId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Purchases", "ApplicationUserId", c => c.String(maxLength: 128));
            CreateIndex("dbo.Purchases", "ApplicationUserId");
            AddForeignKey("dbo.Purchases", "ApplicationUserId", "dbo.AspNetUsers", "Id");
        }
    }
}
