namespace AppTrucking.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class User : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.AspNetUsers", "E_mail");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "E_mail", c => c.String());
        }
    }
}
