namespace OpenDataStorage.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Pilot_v1 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.AspNetUsers", "DateOfBirth");
            DropColumn("dbo.AspNetUsers", "RegisteredDate");
            DropColumn("dbo.AspNetUsers", "LastLoginTime");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "LastLoginTime", c => c.DateTime());
            AddColumn("dbo.AspNetUsers", "RegisteredDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.AspNetUsers", "DateOfBirth", c => c.DateTime(nullable: false));
        }
    }
}
