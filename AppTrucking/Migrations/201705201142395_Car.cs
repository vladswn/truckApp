namespace AppTrucking.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Car : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Cars", "Number", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Cars", "Number");
        }
    }
}
