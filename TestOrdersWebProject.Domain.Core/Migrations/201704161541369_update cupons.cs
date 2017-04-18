namespace TestOrdersWebProject.Domain.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatecupons : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Cupons", "MyProperty");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Cupons", "MyProperty", c => c.Int(nullable: false));
        }
    }
}
