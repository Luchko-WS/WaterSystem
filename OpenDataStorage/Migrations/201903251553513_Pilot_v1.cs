namespace OpenDataStorage.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Pilot_v1 : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.HierarchyObjects", newName: "HierarchyObject");
            AddColumn("dbo.Characteristics", "Type", c => c.Int(nullable: false));
            AddColumn("dbo.HierarchyObject", "Type", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.HierarchyObject", "Type");
            DropColumn("dbo.Characteristics", "Type");
            RenameTable(name: "dbo.HierarchyObject", newName: "HierarchyObjects");
        }
    }
}
