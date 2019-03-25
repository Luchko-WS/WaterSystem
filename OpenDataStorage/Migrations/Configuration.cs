namespace OpenDataStorage.Migrations
{
    using OpenDataStorage.Common;
    using OpenDataStorage.Common.DbContext;
    using OpenDataStorageCore;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(ApplicationDbContext context)
        {
            if(!context.Characteristics.Any(c => c.Level == 0))
            {
                context.Characteristics.Add(new NestedSetsFolder()
                {
                    Level = 0,
                    LeftKey = 1,
                    RightKey = 2,
                    Name = "root",
                    Description = "root node",
                    OwnerId = "system"
                });
            }

            if(!context.HierarchyObjects.Any(o => o.Level == 0))
            {
                context.HierarchyObjects.Add(new NestedSetsFolder()
                {
                    Level = 0,
                    LeftKey = 1,
                    RightKey = 2,
                    Name = "root",
                    Description = "root node",
                    OwnerId = "system"
                });
            }

            CreateStoredProcedurePreCreateNestedSetsNode(context);
            CreateStoredProcedurePostCreateNestedSetsNode(context);
            CreateStoredProcedurePostRemoveNestedSetsNode(context);
            CreateStoredProcedurePreMoveNestedSetsNode(context);
        }

        private void CreateStoredProcedurePreCreateNestedSetsNode(ApplicationDbContext context)
        {
            context.Database.ExecuteSqlCommand(@"IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PreCreateNestedSetsNode]') AND type in (N'P', N'PC'))
            DROP PROCEDURE [dbo].[PreCreateNestedSetsNode]");

            context.Database.ExecuteSqlCommand(@"CREATE PROCEDURE [dbo].[PreCreateNestedSetsNode]
                @TableName nvarchar(50),
                @RightKey int
            AS
            BEGIN
                UPDATE dbo.[@TableName] SET LeftKey = LeftKey + 2, RightKey = RightKey + 2 WHERE LeftKey > @RightKey;
                UPDATE dbo.[@TableName] SET RightKey = RightKey + 2 WHERE RightKey >= @RightKey AND LeftKey < @RightKey;
            END");
        }

        private void CreateStoredProcedurePostCreateNestedSetsNode(ApplicationDbContext context)
        {
            context.Database.ExecuteSqlCommand(@"IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PostCreateNestedSetsNode]') AND type in (N'P', N'PC'))
            DROP PROCEDURE [dbo].[PostCreateNestedSetsNode]");

            context.Database.ExecuteSqlCommand(@"CREATE PROCEDURE [dbo].[PostCreateNestedSetsNode]
                    @TableName nvarchar(50),
                    @RightKey int
            AS
            BEGIN
                UPDATE dbo.[@TableName] SET RightKey = RightKey + 2, LeftKey = CASE WHEN LeftKey > @RightKey THEN LeftKey + 2 ELSE LeftKey END WHERE RightKey >= @RightKey;
            END");
        }

        private void CreateStoredProcedurePostRemoveNestedSetsNode(ApplicationDbContext context)
        {
            context.Database.ExecuteSqlCommand(@"IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PostRemoveNestedSetsNode]') AND type in (N'P', N'PC'))
            DROP PROCEDURE [dbo].[PostRemoveNestedSetsNode]");

            context.Database.ExecuteSqlCommand(@"CREATE PROCEDURE [dbo].[PostRemoveNestedSetsNode]
                @TableName nvarchar(50),
                @LeftKey int,
                @RightKey int
            AS
            BEGIN
                UPDATE dbo.[@TableName] SET RightKey = RightKey – (@RightKey - @LeftKey + 1) WHERE RightKey > @RightKey AND LeftKey < @LeftKey;
                UPDATE dbo.[@TableName] SET LeftKey = LeftKey – (@RightKey - @LeftKey + 1), RightKey = RightKey – (@RightKey - @LeftKey + 1) WHERE LeftKey > @RightKey;
                UPDATE dbo.[@TableName] SET LeftKey = CASE WHEN LeftKey > @LeftKey THEN LeftKey – (@RightKey - @LeftKey + 1) ELSE LeftKey END RightKey = RightKey – (@RightKey - @LeftKey + 1) WHERE RightKey > @RightKey;
            END");
        }

        private void CreateStoredProcedurePreMoveNestedSetsNode(ApplicationDbContext context)
        {
            context.Database.ExecuteSqlCommand(@"IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PreMoveNestedSetsNode]') AND type in (N'P', N'PC'))
            DROP PROCEDURE [dbo].[PostRemoveNestedSetsNode]");

            context.Database.ExecuteSqlCommand(@"CREATE PROCEDURE [dbo].[PostRemoveNestedSetsNode]
                @TableName nvarchar(50),
                @ObjectId uniqueidentifier,
                @NewRightKey int,
                @NewLevel int,
                @oldRightKey int
            AS
            BEGIN
                UPDATE dbo.[@TableName] SET LeftKey=LeftKey+2, RightKey=RightKey+2 WHERE LeftKey > @NewRightKey;
                UPDATE dbo.[@TableName] SET RightKey = RightKey + 2 WHERE RightKey >= @NewRightKey AND LeftKey < @NewRightKey;
                UPDATE dbo.[@TableName] SET LeftKey = @NewRightKey, RightKey = @NewRightKey + 1, node_Level = @NewLevel + 1 WHERE Id = @ObjectId;
                UPDATE dbo.[@TableName] SET RightKey = RightKey - 2 WHERE RightKey > @oldRightKey;
                UPDATE dbo.[@TableName] SET LeftKey = LeftKey - 2 WHERE LeftKey > @oldRightKey;
            END");
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
