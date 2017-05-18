namespace AppTrucking.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MapData : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.MapDatas", "DispatchTime");
        }
        
        public override void Down()
        {
            AddColumn("dbo.MapDatas", "DispatchTime", c => c.DateTime(nullable: false));
        }
    }
}
