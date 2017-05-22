namespace AppTrucking.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Order1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Orders", "IsSent", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Orders", "IsSent");
        }
    }
}
