namespace OpenDataStorage.Migrations
{
    using OpenDataStorage.Common.DbContext;
    using OpenDataStorage.Helpers;
    using OpenDataStorageCore;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
            MigrationsDirectory = @"Migrations";
        }

        protected override void Seed(ApplicationDbContext context)
        {
            if(!context.Characteristics.Any(c => c.Level == 0))
            {
                context.Characteristics.Add(new Characteristic
                {
                    Level = 0,
                    LeftKey = 1,
                    RightKey = 2,
                    Name = "root",
                    Description = "root node",
                    OwnerId = "system",
                    Type = EntityType.Folder
                });
            }

            if(!context.HierarchyObjects.Any(o => o.Level == 0))
            {
                context.HierarchyObjects.Add(new HierarchyObject
                {
                    Level = 0,
                    LeftKey = 1,
                    RightKey = 2,
                    Name = "root",
                    Description = "root node",
                    OwnerId = "system"
                });
            }

            if(!context.ObjectTypes.Any(t => t.Level == 0))
            {
                context.ObjectTypes.Add(new ObjectType
                {
                    Level = 0,
                    LeftKey = 1,
                    RightKey = 2,
                    Name = "root",
                    Description = "root node",
                    OwnerId = "system",
                    Type = EntityType.Folder
                });
            }

            PrepateStoredProceduresForCharacteristics(context);
            PrepateStoredProceduresForHierarchyObjects(context);
            PrepateStoredProceduresForObjectsTypes(context);

            AddConstraintsToHierarchyObjectsTable(context);
        }

        private void AddConstraintsToHierarchyObjectsTable(ApplicationDbContext context)
        {
            var hierarchyObjectsTableName = ((IApplicationDbContext)context).HierarchyObjectContext.TableName;
            var typesTableName = ((IApplicationDbContext)context).ObjectTypeContext.TableName;

            var primaryKey = ReflectionHelper.GetPropName((ObjectType t) => t.Id);
            var foreignKey = ReflectionHelper.GetPropName((HierarchyObject o) => o.ObjectTypeId);
            var constraintName = string.Format("FK_dbo.{0}_dbo.{1}_{2}", hierarchyObjectsTableName, typesTableName, foreignKey);

            context.Database.ExecuteSqlCommand($"ALTER TABLE [dbo].[{hierarchyObjectsTableName}] DROP CONSTRAINT [{constraintName}]");
            context.Database.ExecuteSqlCommand($"ALTER TABLE [dbo].[{hierarchyObjectsTableName}] ADD CONSTRAINT [{constraintName}] FOREIGN KEY ([{foreignKey}]) REFERENCES [dbo].[{typesTableName}] ([{primaryKey}]) ON DELETE SET NULL");
        }

        private void PrepateStoredProceduresForHierarchyObjects(ApplicationDbContext context)
        {
            string tableName = ((IApplicationDbContext)context).HierarchyObjectContext.TableName;
            CreateStoredProcedurePreCreateNestedSetsNode(context, tableName);
            CreateStoredProcedurePostRemoveNestedSetsNode(context, tableName);
            CreateStoredProcedureMoveToLeftNestedSetsNode(context, tableName);
            CreateStoredProcedureMoveToRightNestedSetsNode(context, tableName);
        }

        private void PrepateStoredProceduresForCharacteristics(ApplicationDbContext context)
        {
            string tableName = ((IApplicationDbContext)context).CharacteristicObjectContext.TableName;
            CreateStoredProcedurePreCreateNestedSetsNode(context, tableName);
            CreateStoredProcedurePostRemoveNestedSetsNode(context, tableName);
            CreateStoredProcedureMoveToLeftNestedSetsNode(context, tableName);
            CreateStoredProcedureMoveToRightNestedSetsNode(context, tableName);
        }

        private void PrepateStoredProceduresForObjectsTypes(ApplicationDbContext context)
        {
            string tableName = ((IApplicationDbContext)context).ObjectTypeContext.TableName;
            CreateStoredProcedurePreCreateNestedSetsNode(context, tableName);
            CreateStoredProcedurePostRemoveNestedSetsNode(context, tableName);
            CreateStoredProcedureMoveToLeftNestedSetsNode(context, tableName);
            CreateStoredProcedureMoveToRightNestedSetsNode(context, tableName);
        }

        private void CreateStoredProcedurePreCreateNestedSetsNode(ApplicationDbContext context, string tableName)
        {
            string procedureName = string.Format("{0}PreCreateNestedSetsNode", tableName);

            context.Database.ExecuteSqlCommand(string.Format(@"IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[{0}]') AND type in (N'P', N'PC'))
            DROP PROCEDURE [dbo].[{0}]", procedureName));

            context.Database.ExecuteSqlCommand(string.Format(@"CREATE PROCEDURE [dbo].[{0}]
                @RightKey int
            AS
            BEGIN
                UPDATE dbo.[{1}] SET LeftKey = LeftKey + 2, RightKey = RightKey + 2 WHERE LeftKey > @RightKey;
                UPDATE dbo.[{1}] SET RightKey = RightKey + 2 WHERE RightKey >= @RightKey AND LeftKey < @RightKey;
            END", procedureName, tableName));
        }

        private void CreateStoredProcedurePostRemoveNestedSetsNode(ApplicationDbContext context, string tableName)
        {
            string procedureName = string.Format("{0}PostRemoveNestedSetsNode", tableName);

            context.Database.ExecuteSqlCommand(string.Format(@"IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[{0}]') AND type in (N'P', N'PC'))
            DROP PROCEDURE [dbo].[{0}]", procedureName));

            context.Database.ExecuteSqlCommand(string.Format(@"CREATE PROCEDURE [dbo].[{0}]
                @LeftKey int,
                @RightKey int
            AS
            BEGIN
                UPDATE dbo.[{1}] SET RightKey = RightKey - (@RightKey - @LeftKey + 1) WHERE RightKey > @RightKey AND LeftKey < @LeftKey;
                UPDATE dbo.[{1}] SET LeftKey = LeftKey - (@RightKey - @LeftKey + 1), RightKey = RightKey - (@RightKey - @LeftKey + 1) WHERE LeftKey > @RightKey;
            END", procedureName, tableName));
        }

        private void CreateStoredProcedureMoveToLeftNestedSetsNode(ApplicationDbContext context, string tableName)
        {
            string procedureName = string.Format("{0}MoveToLeftNestedSetsNode", tableName);

            context.Database.ExecuteSqlCommand(string.Format(@"IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[{0}]') AND type in (N'P', N'PC'))
            DROP PROCEDURE [dbo].[{0}]", procedureName));

            context.Database.ExecuteSqlCommand(string.Format(@"CREATE PROCEDURE [dbo].[{0}]
                @ParentRightKey int,
                @ParentLevel int,
                @NodeLeftKey int,
                @NodeRightKey int,
                @NodeLevel int
            AS
            BEGIN
                UPDATE dbo.[{1}] SET LeftKey=LeftKey+(@NodeRightKey - @NodeLeftKey + 1), RightKey=RightKey+(@NodeRightKey - @NodeLeftKey + 1) WHERE LeftKey > @ParentRightKey;
                UPDATE dbo.[{1}] SET RightKey=RightKey + (@NodeRightKey - @NodeLeftKey + 1) WHERE RightKey >= @ParentRightKey AND LeftKey < @ParentRightKey;
                UPDATE dbo.[{1}] SET LeftKey=LeftKey-(@NodeLeftKey + (@NodeRightKey - @NodeLeftKey + 1))+@ParentRightKey, RightKey=RightKey-(@NodeLeftKey + (@NodeRightKey - @NodeLeftKey + 1)) + @ParentRightKey, Level=Level-@NodeLevel + (@ParentLevel + 1) WHERE LeftKey >= (@NodeLeftKey + (@NodeRightKey - @NodeLeftKey + 1)) AND RightKey <= (@NodeRightKey + (@NodeRightKey - @NodeLeftKey + 1));
                UPDATE dbo.[{1}] SET RightKey = RightKey - (@NodeRightKey - @NodeLeftKey + 1) WHERE RightKey > (@NodeRightKey + (@NodeRightKey - @NodeLeftKey + 1));
                UPDATE dbo.[{1}] SET LeftKey = LeftKey - (@NodeRightKey - @NodeLeftKey + 1) WHERE LeftKey > (@NodeRightKey + (@NodeRightKey - @NodeLeftKey + 1));
            END", procedureName, tableName));
        }

        private void CreateStoredProcedureMoveToRightNestedSetsNode(ApplicationDbContext context, string tableName)
        {
            string procedureName = string.Format("{0}MoveToRightNestedSetsNode", tableName);

            context.Database.ExecuteSqlCommand(string.Format(@"IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[{0}]') AND type in (N'P', N'PC'))
            DROP PROCEDURE [dbo].[{0}]", procedureName));

            context.Database.ExecuteSqlCommand(string.Format(@"CREATE PROCEDURE [dbo].[{0}]
                @ParentRightKey int,
                @ParentLevel int,
                @NodeLeftKey int,
                @NodeRightKey int,
                @NodeLevel int
            AS
            BEGIN
                UPDATE dbo.[{1}] SET LeftKey=LeftKey+(@NodeRightKey - @NodeLeftKey + 1), RightKey=RightKey+(@NodeRightKey - @NodeLeftKey + 1) WHERE LeftKey > @ParentRightKey;
                UPDATE dbo.[{1}] SET RightKey=RightKey + (@NodeRightKey - @NodeLeftKey + 1) WHERE RightKey >= @ParentRightKey AND LeftKey < @ParentRightKey;
                UPDATE dbo.[{1}] SET LeftKey=LeftKey-@NodeLeftKey+@ParentRightKey, RightKey=RightKey-@NodeLeftKey+@ParentRightKey, Level=Level-@NodeLevel+ (@ParentLevel + 1) WHERE LeftKey>=@NodeLeftKey AND RightKey<=@NodeRightKey;
                UPDATE dbo.[{1}] SET RightKey = RightKey - (@NodeRightKey - @NodeLeftKey + 1) WHERE RightKey > @NodeRightKey;
                UPDATE dbo.[{1}] SET LeftKey = LeftKey - (@NodeRightKey - @NodeLeftKey + 1) WHERE LeftKey > @NodeRightKey;
            END", procedureName, tableName));
        }
    }
}
