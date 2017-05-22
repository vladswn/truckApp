namespace AppTrucking.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Order_Car : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Cars", "IsFree", c => c.Boolean(nullable: false));
            AddColumn("dbo.Orders", "Weight", c => c.Int(nullable: false));
            AddColumn("dbo.Orders", "volume", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Orders", "volume");
            DropColumn("dbo.Orders", "Weight");
            DropColumn("dbo.Cars", "IsFree");
        }
    }
}
