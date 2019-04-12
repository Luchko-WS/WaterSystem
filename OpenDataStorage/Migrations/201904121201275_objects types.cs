namespace OpenDataStorage.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class objectstypes : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.HierarchyObjectType", newName: "Types");
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.Types", newName: "HierarchyObjectType");
        }
    }
}
