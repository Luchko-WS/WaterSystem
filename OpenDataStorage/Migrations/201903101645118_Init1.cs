namespace OpenDataStorage.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Init1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.HierarchyObjectCharacteristics", "HierarchyObject_Id", "dbo.HierarchyObjects");
            DropForeignKey("dbo.HierarchyObjectCharacteristics", "Characteristic_Id", "dbo.Characteristics");
            DropIndex("dbo.HierarchyObjectCharacteristics", new[] { "HierarchyObject_Id" });
            DropIndex("dbo.HierarchyObjectCharacteristics", new[] { "Characteristic_Id" });
            DropTable("dbo.HierarchyObjectCharacteristics");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.HierarchyObjectCharacteristics",
                c => new
                    {
                        HierarchyObject_Id = c.Guid(nullable: false),
                        Characteristic_Id = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.HierarchyObject_Id, t.Characteristic_Id });
            
            CreateIndex("dbo.HierarchyObjectCharacteristics", "Characteristic_Id");
            CreateIndex("dbo.HierarchyObjectCharacteristics", "HierarchyObject_Id");
            AddForeignKey("dbo.HierarchyObjectCharacteristics", "Characteristic_Id", "dbo.Characteristics", "Id", cascadeDelete: true);
            AddForeignKey("dbo.HierarchyObjectCharacteristics", "HierarchyObject_Id", "dbo.HierarchyObjects", "Id", cascadeDelete: true);
        }
    }
}
