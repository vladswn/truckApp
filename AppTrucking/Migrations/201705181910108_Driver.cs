namespace AppTrucking.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Driver : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Drivers",
                c => new
                    {
                        DriverId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Surname = c.String(),
                        Skype = c.String(maxLength: 15),
                        Viber = c.String(),
                        Telephone = c.String(nullable: false),
                        CarId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.DriverId)
                .ForeignKey("dbo.Cars", t => t.CarId, cascadeDelete: true)
                .Index(t => t.CarId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Drivers", "CarId", "dbo.Cars");
            DropIndex("dbo.Drivers", new[] { "CarId" });
            DropTable("dbo.Drivers");
        }
    }
}
