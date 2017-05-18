namespace AppTrucking.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Order : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MapDatas", "DispatchTime", c => c.DateTime(nullable: false));
            AddColumn("dbo.MapDatas", "ArrivalTime", c => c.DateTime(nullable: false));
            AlterColumn("dbo.MapDatas", "Distance", c => c.Double(nullable: false));
            DropColumn("dbo.Orders", "DispatchTime");
            DropColumn("dbo.Orders", "ArrivalTime");
            DropColumn("dbo.MapDatas", "Time");
        }
        
        public override void Down()
        {
            AddColumn("dbo.MapDatas", "Time", c => c.String());
            AddColumn("dbo.Orders", "ArrivalTime", c => c.DateTime(nullable: false));
            AddColumn("dbo.Orders", "DispatchTime", c => c.DateTime(nullable: false));
            AlterColumn("dbo.MapDatas", "Distance", c => c.String());
            DropColumn("dbo.MapDatas", "ArrivalTime");
            DropColumn("dbo.MapDatas", "DispatchTime");
        }
    }
}
