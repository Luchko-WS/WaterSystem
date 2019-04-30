namespace OpenDataStorage.Migrations
{
    using OpenDataStorage.Common.DbContext;
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
            PrepateStoredProceduresForHierarchyObjectsTypes(context);
        }

        private void PrepateStoredProceduresForHierarchyObjects(ApplicationDbContext context)
        {
            string tableName = ((IApplicationDbContext)context).HierarchyObjectContext.TableName;
            CreateStoredProcedurePreCreateNestedSetsNode(context, tableName);
            CreateStoredProcedurePostRemoveNestedSetsNode(context, tableName);
            CreateStoredProcedurePreMoveNestedSetsNode(context, tableName);
        }

        private void PrepateStoredProceduresForCharacteristics(ApplicationDbContext context)
        {
            string tableName = ((IApplicationDbContext)context).CharacteristicObjectContext.TableName;
            CreateStoredProcedurePreCreateNestedSetsNode(context, tableName);
            CreateStoredProcedurePostRemoveNestedSetsNode(context, tableName);
            CreateStoredProcedurePreMoveNestedSetsNode(context, tableName);
        }

        private void PrepateStoredProceduresForHierarchyObjectsTypes(ApplicationDbContext context)
        {
            string tableName = ((IApplicationDbContext)context).ObjectTypeContext.TableName;
            CreateStoredProcedurePreCreateNestedSetsNode(context, tableName);
            CreateStoredProcedurePostRemoveNestedSetsNode(context, tableName);
            CreateStoredProcedurePreMoveNestedSetsNode(context, tableName);
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
                UPDATE dbo.[{1}] SET LeftKey = CASE WHEN LeftKey > @LeftKey THEN LeftKey - (@RightKey - @LeftKey + 1) ELSE LeftKey END, RightKey = RightKey - (@RightKey - @LeftKey + 1) WHERE RightKey > @RightKey;
            END", procedureName, tableName));
        }

        private void CreateStoredProcedurePreMoveNestedSetsNode(ApplicationDbContext context, string tableName)
        {
            string procedureName = string.Format("{0}PreMoveNestedSetsNode", tableName);

            context.Database.ExecuteSqlCommand(string.Format(@"IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[{0}]') AND type in (N'P', N'PC'))
            DROP PROCEDURE [dbo].[{0}]", procedureName));

            context.Database.ExecuteSqlCommand(string.Format(@"CREATE PROCEDURE [dbo].[{0}]
                @ObjectId uniqueidentifier,
                @NewRightKey int,
                @NewLevel int,
                @oldRightKey int
            AS
            BEGIN
                UPDATE dbo.[{1}] SET LeftKey=LeftKey+2, RightKey=RightKey+2 WHERE LeftKey > @NewRightKey;
                UPDATE dbo.[{1}] SET RightKey = RightKey + 2 WHERE RightKey >= @NewRightKey AND LeftKey < @NewRightKey;
                UPDATE dbo.[{1}] SET LeftKey = @NewRightKey, RightKey = @NewRightKey + 1, Level = @NewLevel + 1 WHERE Id = @ObjectId;
                UPDATE dbo.[{1}] SET RightKey = RightKey - 2 WHERE RightKey > @oldRightKey;
                UPDATE dbo.[{1}] SET LeftKey = LeftKey - 2 WHERE LeftKey > @oldRightKey;
            END", procedureName, tableName));
        }

        /*
        private void CreateStoredProcedureGetChildNestedSetsNodes(ApplicationDbContext context)
        {
            context.Database.ExecuteSqlCommand(@"IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetQueenGeneralInfoPublic]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
			DROP FUNCTION [dbo].[GetQueenGeneralInfoPublic]");

            context.Database.ExecuteSqlCommand(@"create function [dbo].[GetQueenGeneralInfoPublic](@QueenId UNIQUEIDENTIFIER)
			RETURNS TABLE
			RETURN
			select 
            g.[Id],
            [Tolerance],
            [HoneyProductivity],
            [WinteringQuality],
            [BroodDensity],
            [MiteResistance],
            [Clearness],
            [InspectionCalmness],
            [SpringGrowth],
            [Swarming],
            [Fertility],
            [WaxProductivity],
            [PollenProductivity],
            [RoyalJellyProductivity],
            [PropolisBuilding],
            [ColonyPower],
            [BeesVitality],
            [BroodVitality],
            g.[AdditionalInfo],
            DiscoidPercentPositive,
            DiscoidPercentNegative
            from[dbo].[QueenGeneralInfoes] as g
            join [dbo].[Queens] as q on g.Id=q.GeneralInfo_Id
            WHERE @QueenId=q.Id");
        }*/
        /*
        private void CreateStoredProcedureGetRootNestedSetNode(ApplicationDbContext context)
        {
            context.Database.ExecuteSqlCommand(@"IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetQueenGeneralInfoPublic]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
			DROP FUNCTION [dbo].[GetQueenGeneralInfoPublic]");

            context.Database.ExecuteSqlCommand(@"create function [dbo].[GetQueenGeneralInfoPublic](@QueenId UNIQUEIDENTIFIER)
			RETURNS TABLE
			RETURN
			select 
            g.[Id],
            [Tolerance],
            [HoneyProductivity],
            [WinteringQuality],
            [BroodDensity],
            [MiteResistance],
            [Clearness],
            [InspectionCalmness],
            [SpringGrowth],
            [Swarming],
            [Fertility],
            [WaxProductivity],
            [PollenProductivity],
            [RoyalJellyProductivity],
            [PropolisBuilding],
            [ColonyPower],
            [BeesVitality],
            [BroodVitality],
            g.[AdditionalInfo],
            DiscoidPercentPositive,
            DiscoidPercentNegative
            from[dbo].[QueenGeneralInfoes] as g
            join [dbo].[Queens] as q on g.Id=q.GeneralInfo_Id
            WHERE @QueenId=q.Id");
        }
        */
        /*
         

        Task<ICollection<T>> GetRootNodes(T node);

        Task<string> GetPath(T destinationNode);

         */
    }
}
