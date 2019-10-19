using OpenDataStorage.Core.DataAccessLayer.DbContext;
using OpenDataStorage.Core.DataAccessLayer.DbSetManagers.NestedSetsEntityManagers.Core;
using OpenDataStorage.Core.Entities.NestedSets;

namespace OpenDataStorage.Core.DataAccessLayer.Common
{
    internal sealed class StoredProceduresManager
    {
        private readonly string PreCreateNestedSetsNodeSpNameTemplate = "{0}PreCreateNestedSetsNode";
        private readonly string PostRemoveNestedSetsNodeSpNameTemplate = "{0}PostRemoveNestedSetsNode";
        private readonly string MoveNestedSetsNodeSpNameTemplate = "{0}MoveNestedSetsNode";

        private StoredProceduresManager() { }

        private static StoredProceduresManager _instance;
        public static StoredProceduresManager Instance
        {
            get
            {
                if(_instance == null)
                {
                    _instance = new StoredProceduresManager();
                }
                return _instance;
            }
        }

        public string GetPreCreateNestedSetsNodeSpName<T>(INestedSetsDbSetManager<T> manager)
            where T : NestedSetsEntity
        {
            return string.Format(PreCreateNestedSetsNodeSpNameTemplate, manager.TableName);
        }

        public string GetPostRemoveNestedSetsNodeSpName<T>(INestedSetsDbSetManager<T> manager)
            where T : NestedSetsEntity
        {
            return string.Format(PostRemoveNestedSetsNodeSpNameTemplate, manager.TableName);
        }

        public string GetMoveNestedSetsNodeSpName<T>(INestedSetsDbSetManager<T> manager)
            where T : NestedSetsEntity
        {
            return string.Format(MoveNestedSetsNodeSpNameTemplate, manager.TableName);
        }

        public void CreateStoredProcedures<T>(IDbContainer dbContainer, INestedSetsDbSetManager<T> manager)
            where T : NestedSetsEntity
        {
            CreateStoredProcedurePreCreateNestedSetsNode(dbContainer, manager);
            CreateStoredProcedurePostRemoveNestedSetsNode(dbContainer, manager);
            CreateStoredProcedureMoveNestedSetsNode(dbContainer, manager);
        }

        private void CreateStoredProcedurePreCreateNestedSetsNode<T>(IDbContainer dbContainer, INestedSetsDbSetManager<T> manager)
            where T : NestedSetsEntity
        {
            string procedureName = GetPreCreateNestedSetsNodeSpName(manager);

            dbContainer.Database.ExecuteSqlCommand(string.Format(@"IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[{0}]') AND type in (N'P', N'PC'))
            DROP PROCEDURE [dbo].[{0}]", procedureName));

            dbContainer.Database.ExecuteSqlCommand(string.Format(@"CREATE PROCEDURE [dbo].[{0}]
                @RightKey int
            AS
            BEGIN
                UPDATE dbo.[{1}] SET LeftKey = LeftKey + 2, RightKey = RightKey + 2 WHERE LeftKey > @RightKey;
                UPDATE dbo.[{1}] SET RightKey = RightKey + 2 WHERE RightKey >= @RightKey AND LeftKey < @RightKey;
            END", procedureName, manager.TableName));
        }

        private void CreateStoredProcedurePostRemoveNestedSetsNode<T>(IDbContainer dbContainer, INestedSetsDbSetManager<T> manager)
            where T : NestedSetsEntity
        {
            string procedureName = GetPostRemoveNestedSetsNodeSpName(manager);

            dbContainer.Database.ExecuteSqlCommand(string.Format(@"IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[{0}]') AND type in (N'P', N'PC'))
            DROP PROCEDURE [dbo].[{0}]", procedureName));

            dbContainer.Database.ExecuteSqlCommand(string.Format(@"CREATE PROCEDURE [dbo].[{0}]
                @LeftKey int,
                @RightKey int
            AS
            BEGIN
                UPDATE dbo.[{1}] SET RightKey = RightKey - (@RightKey - @LeftKey + 1) WHERE RightKey > @RightKey AND LeftKey < @LeftKey;
                UPDATE dbo.[{1}] SET LeftKey = LeftKey - (@RightKey - @LeftKey + 1), RightKey = RightKey - (@RightKey - @LeftKey + 1) WHERE LeftKey > @RightKey;
            END", procedureName, manager.TableName));
        }

        private void CreateStoredProcedureMoveNestedSetsNode<T>(IDbContainer dbContainer, INestedSetsDbSetManager<T> manager)
            where T : NestedSetsEntity
        {
            string procedureName = GetMoveNestedSetsNodeSpName(manager);

            dbContainer.Database.ExecuteSqlCommand(string.Format(@"IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[{0}]') AND type in (N'P', N'PC'))
            DROP PROCEDURE [dbo].[{0}]", procedureName));

            dbContainer.Database.ExecuteSqlCommand(string.Format(@"CREATE PROCEDURE [dbo].[{0}]
                @NodeLeftKey int,
                @NodeRightKey int,
                @NodeLevel int,
                @ParentLeftKey int,
                @ParentRightKey int,
                @ParentLevel int
            AS
            BEGIN
                DECLARE @d int = @NodeRightKey - @NodeLeftKey + 1;
                UPDATE dbo.[{1}] SET LeftKey = 0 - LeftKey, RightKey = 0 - RightKey WHERE LeftKey >= @NodeLeftKey AND RightKey <= @NodeRightKey;
                UPDATE dbo.[{1}] SET LeftKey = LeftKey - @d WHERE LeftKey > @NodeRightKey;
                UPDATE dbo.[{1}] SET RightKey = RightKey - @d WHERE RightKey > @NodeRightKey;

                DECLARE @pr int = CASE WHEN @ParentRightKey > @NodeRightKey THEN @ParentRightKey - (@NodeRightKey - @NodeLeftKey + 1) ELSE @ParentRightKey END;
                UPDATE dbo.[{1}] SET LeftKey = LeftKey + @d WHERE LeftKey >= @pr;
                UPDATE dbo.[{1}] SET RightKey = RightKey + @d WHERE RightKey >= @pr;

                DECLARE @pd int = CASE WHEN @ParentRightKey > @NodeRightKey THEN @ParentRightKey - @NodeRightKey - 1 ELSE @ParentRightKey - @NodeRightKey - 1 + @d END;
                DECLARE @dl int = @ParentLevel + 1 - @NodeLevel;
                UPDATE dbo.[{1}] SET LeftKey = @pd - LeftKey, RightKey = @pd - RightKey, Level = Level + @dl WHERE LeftKey <= 0-@NodeLeftKey AND RightKey >= 0-@NodeRightKey;
            END", procedureName, manager.TableName));
        }
    }
}
