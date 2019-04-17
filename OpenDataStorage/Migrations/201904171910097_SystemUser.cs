namespace OpenDataStorage.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SystemUser : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "RegisteredDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.AspNetUsers", "LastLoginTime", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "LastLoginTime");
            DropColumn("dbo.AspNetUsers", "RegisteredDate");
        }
    }
}
