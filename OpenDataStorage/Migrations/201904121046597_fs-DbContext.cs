namespace OpenDataStorage.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fsDbContext : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.HierarchyObject", "Type");
        }
        
        public override void Down()
        {
            AddColumn("dbo.HierarchyObject", "Type", c => c.Int(nullable: false));
        }
    }
}
