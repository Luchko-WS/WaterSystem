using OpenDataStorageCore;
using System;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace OpenDataStorage.Common.DbContext
{
    public class CharacteristicDbContextManager : BaseNestedSetsDbContextManager<Characteristic>
    {
        private ApplicationDbContext _dbContext;

        public CharacteristicDbContextManager(ApplicationDbContext dbContext)
            : base(dbContext.Characteristics)
        {
            _dbContext = dbContext;
            TableName = "Characteristics";
        }

        protected override async Task PreAdd(Characteristic @object, NestedSetsFolder parentFolder)
        {
            await PreCreateNestedNode(parentFolder.RightKey);
        }

        protected override async Task PostAdd(Characteristic @object, NestedSetsFolder parentFolder)
        {
            await PostCreateNestedNode(parentFolder.RightKey);
        }

        protected override async Task PreAddFolder(NestedSetsFolder folder, NestedSetsFolder parentFolder)
        {
            await PreCreateNestedNode(parentFolder.RightKey);
        }

        protected override async Task PostAddFolder(NestedSetsFolder folder, NestedSetsFolder parentFolder)
        {
            await PostCreateNestedNode(parentFolder.RightKey);
        }

        private async Task PreCreateNestedNode(int parentRightKey)
        {
            var tableNameParam = new SqlParameter { ParameterName = "TableName", Value = TableName };
            var rightKeyParam = new SqlParameter { ParameterName = "RightKey", Value = parentRightKey };
            await _dbContext.Database.ExecuteSqlCommandAsync("exec dbo.PreCreateNestedSetsNode @TableName, @RightKey", tableNameParam, rightKeyParam);
        }

        private async Task PostCreateNestedNode(int parentRightKey)
        {
            var tableNameParam = new SqlParameter { ParameterName = "TableName", Value = TableName };
            var rightKeyParam = new SqlParameter { ParameterName = "RightKey", Value = parentRightKey };
            await _dbContext.Database.ExecuteSqlCommandAsync("exec dbo.PostCreateNestedSetsNode @TableName, @RightKey", tableNameParam, rightKeyParam);
        }

        protected override async Task PreRemove(Characteristic @object)
        {
        }

        protected override async Task PostRemove(Characteristic @object)
        {
            await PostRemoveNestedSetsNode(@object.LeftKey, @object.RightKey);
        }

        protected override async Task PreRemoveFolder(NestedSetsFolder folder)
        {
        }

        protected override async Task PostRemoveFolder(NestedSetsFolder folder)
        {
            await PostRemoveNestedSetsNode(folder.LeftKey, folder.RightKey);
        }

        private async Task PostRemoveNestedSetsNode(int leftKey, int rightKey)
        {
            var tableNameParam = new SqlParameter { ParameterName = "TableName", Value = TableName };
            var leftKeyParam = new SqlParameter { ParameterName = "LeftKey", Value = leftKey };
            var rightKeyParam = new SqlParameter { ParameterName = "RightKey", Value = rightKey };
            await _dbContext.Database.ExecuteSqlCommandAsync("exec dbo.PostRemoveNestedSetsNode @TableName, @LeftKey, @RightKey", tableNameParam, leftKeyParam, rightKeyParam);
        }


        protected override Task PreMove()
        {
            throw new NotImplementedException();
        }

        protected override Task PostMove()
        {
            throw new NotImplementedException();
        }

        protected override Task PreMoveFolder()
        {
            throw new NotImplementedException();
        }

        protected override Task PostMoveFolder()
        {
            throw new NotImplementedException();
        }
    }
}