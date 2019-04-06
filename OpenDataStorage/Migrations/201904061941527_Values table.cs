namespace OpenDataStorage.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Valuestable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Values",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Value = c.String(),
                        HierarchyObjectId = c.Guid(nullable: false),
                        CharacterisitcId = c.Guid(nullable: false),
                        OwnerId = c.String(),
                        Characteristic_Id = c.Guid(nullable: false),
                        HierarchyObject_Id = c.Guid(),
                        Characteristic_Id1 = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Characteristics", t => t.Characteristic_Id, cascadeDelete: true)
                .ForeignKey("dbo.HierarchyObject", t => t.HierarchyObject_Id)
                .ForeignKey("dbo.HierarchyObject", t => t.HierarchyObjectId, cascadeDelete: true)
                .ForeignKey("dbo.Characteristics", t => t.Characteristic_Id1)
                .Index(t => t.HierarchyObjectId)
                .Index(t => t.Characteristic_Id)
                .Index(t => t.HierarchyObject_Id)
                .Index(t => t.Characteristic_Id1);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Values", "Characteristic_Id1", "dbo.Characteristics");
            DropForeignKey("dbo.Values", "HierarchyObjectId", "dbo.HierarchyObject");
            DropForeignKey("dbo.Values", "HierarchyObject_Id", "dbo.HierarchyObject");
            DropForeignKey("dbo.Values", "Characteristic_Id", "dbo.Characteristics");
            DropIndex("dbo.Values", new[] { "Characteristic_Id1" });
            DropIndex("dbo.Values", new[] { "HierarchyObject_Id" });
            DropIndex("dbo.Values", new[] { "Characteristic_Id" });
            DropIndex("dbo.Values", new[] { "HierarchyObjectId" });
            DropTable("dbo.Values");
        }
    }
}
