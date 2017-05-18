namespace AppTrucking.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Duration : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MapDatas", "Duration", c => c.Double(nullable: false));
            DropColumn("dbo.MapDatas", "ArrivalTime");
        }
        
        public override void Down()
        {
            AddColumn("dbo.MapDatas", "ArrivalTime", c => c.DateTime(nullable: false));
            DropColumn("dbo.MapDatas", "Duration");
        }
    }
}
