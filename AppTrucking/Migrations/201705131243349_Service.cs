namespace AppTrucking.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Service : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Services", "Price", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Services", "Price");
        }
    }
}
