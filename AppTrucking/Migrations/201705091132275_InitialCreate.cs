namespace AppTrucking.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Cars",
                c => new
                    {
                        CarId = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false),
                        LiftingCapacity = c.String(nullable: false),
                        BodyVolume = c.Int(nullable: false),
                        Tonnage = c.Double(nullable: false),
                        Prce = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TypeOfTransportId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.CarId)
                .ForeignKey("dbo.TypeOfTransports", t => t.TypeOfTransportId, cascadeDelete: true)
                .Index(t => t.TypeOfTransportId);
            
            CreateTable(
                "dbo.Orders",
                c => new
                    {
                        OrderId = c.Int(nullable: false, identity: true),
                        OrderTime = c.DateTime(nullable: false),
                        DispatchTime = c.DateTime(nullable: false),
                        ArrivalTime = c.DateTime(nullable: false),
                        CarId = c.Int(nullable: false),
                        Total = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Status = c.Boolean(nullable: false),
                        Description = c.String(),
                        ApplicationUserId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.OrderId)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUserId)
                .ForeignKey("dbo.Cars", t => t.CarId, cascadeDelete: true)
                .Index(t => t.CarId)
                .Index(t => t.ApplicationUserId);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        CompanyName = c.String(),
                        Name = c.String(),
                        Surname = c.String(),
                        E_mail = c.String(),
                        Skype = c.String(maxLength: 15),
                        Viber = c.String(),
                        Telephone = c.String(),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.MapDatas",
                c => new
                    {
                        MapDataId = c.Int(nullable: false, identity: true),
                        From = c.String(),
                        To = c.String(),
                        Distance = c.String(),
                        Time = c.String(),
                        OrderId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.MapDataId)
                .ForeignKey("dbo.Orders", t => t.OrderId, cascadeDelete: true)
                .Index(t => t.OrderId);
            
            CreateTable(
                "dbo.Services",
                c => new
                    {
                        ServiceId = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.ServiceId);
            
            CreateTable(
                "dbo.TypeOfTransports",
                c => new
                    {
                        TypeOfTransportId = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.TypeOfTransportId);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                        Description = c.String(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.ServiceOrders",
                c => new
                    {
                        Service_ServiceId = c.Int(nullable: false),
                        Order_OrderId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Service_ServiceId, t.Order_OrderId })
                .ForeignKey("dbo.Services", t => t.Service_ServiceId, cascadeDelete: true)
                .ForeignKey("dbo.Orders", t => t.Order_OrderId, cascadeDelete: true)
                .Index(t => t.Service_ServiceId)
                .Index(t => t.Order_OrderId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.Cars", "TypeOfTransportId", "dbo.TypeOfTransports");
            DropForeignKey("dbo.ServiceOrders", "Order_OrderId", "dbo.Orders");
            DropForeignKey("dbo.ServiceOrders", "Service_ServiceId", "dbo.Services");
            DropForeignKey("dbo.MapDatas", "OrderId", "dbo.Orders");
            DropForeignKey("dbo.Orders", "CarId", "dbo.Cars");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Orders", "ApplicationUserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.ServiceOrders", new[] { "Order_OrderId" });
            DropIndex("dbo.ServiceOrders", new[] { "Service_ServiceId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.MapDatas", new[] { "OrderId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.Orders", new[] { "ApplicationUserId" });
            DropIndex("dbo.Orders", new[] { "CarId" });
            DropIndex("dbo.Cars", new[] { "TypeOfTransportId" });
            DropTable("dbo.ServiceOrders");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.TypeOfTransports");
            DropTable("dbo.Services");
            DropTable("dbo.MapDatas");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.Orders");
            DropTable("dbo.Cars");
        }
    }
}
