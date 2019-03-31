using OpenDataStorageCore;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace OpenDataStorage.Common.DbContext
{
    public abstract class BaseNestedSetsDbContextManager<T> : INestedSetsObjectContext<T> where T : NestedSetsObject
    {
        protected DbSet<T> _dbSet;
        protected ApplicationDbContext _dbContext;

        public string TableName { get; protected set; }

        public BaseNestedSetsDbContextManager(ApplicationDbContext dbContext, DbSet<T> dbSet)
        {
            _dbContext = dbContext;
            _dbSet = dbSet;
        }

        public IQueryable<T> Entities => this._dbSet;

        protected abstract Task AddObjectInternal(T @object);
        public async Task AddObject(T @object, NestedSetsFileSystemEntity parentFolder)
        {
            @object.LeftKey = parentFolder.RightKey;
            @object.RightKey = parentFolder.RightKey + 1;
            @object.Level = parentFolder.Level + 1;

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    var rightKeyParam = new SqlParameter { ParameterName = "RightKey", Value = parentFolder.RightKey };
                    await _dbContext.Database.ExecuteSqlCommandAsync("exec dbo." + TableName + "PreCreateNestedSetsNode @RightKey", rightKeyParam);
                    await AddObjectInternal(@object);
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
            }
        }
        public async Task AddObject(T @object, Guid folderId)
        {
            var folder = _dbSet.FirstOrDefault(f => f.Id == folderId && f.Type == EntityType.Folder);
            if (folder != null)
            {
                await AddObject(@object, folder);
            }
            throw new ArgumentException(string.Format("Folder with id = {0} not found in {1} table.", folderId, TableName));
        }

        protected abstract Task UpdateObjectInternal(T @object);
        public async Task UpdateObject(T @object)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    await UpdateObjectInternal(@object);
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
            }
        }

        protected abstract Task MoveObjetInternal();
        public async Task MoveObject(T @object, NestedSetsFileSystemEntity newFolder)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    await MoveObjetInternal();
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
            }
        }
        public async Task MoveObject(T @bject, Guid newFolderId)
        {
            var folder = _dbSet.FirstOrDefault(f => f.Id == newFolderId && f.Type == EntityType.Folder);
            if (folder != null)
            {
                await MoveObject(@bject, folder);
            }
            throw new ArgumentException(string.Format("Folder with id = {0} not found in {1} table.", newFolderId, TableName));
        }

        protected abstract Task RemoveObjectInternal(T @object);
        public async Task RemoveObject(T @object)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    await RemoveObjectInternal(@object);
                    var leftKeyParam = new SqlParameter { ParameterName = "LeftKey", Value = @object.LeftKey };
                    var rightKeyParam = new SqlParameter { ParameterName = "RightKey", Value = @object.RightKey };
                    await _dbContext.Database.ExecuteSqlCommandAsync("exec dbo." + TableName + "PostRemoveNestedSetsNode @LeftKey, @RightKey", leftKeyParam, rightKeyParam);
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
            }
        }

        //folder
        protected abstract Task AddFolderInternal(NestedSetsFileSystemEntity folder);
        public async Task AddFolder(NestedSetsFileSystemEntity folder, NestedSetsFileSystemEntity parentFolder)
        {
            folder.LeftKey = parentFolder.RightKey;
            folder.RightKey = parentFolder.RightKey + 1;
            folder.Level = parentFolder.Level + 1;

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    var rightKeyParam = new SqlParameter { ParameterName = "RightKey", Value = parentFolder.RightKey };
                    await _dbContext.Database.ExecuteSqlCommandAsync("exec dbo." + TableName + "PreCreateNestedSetsNode @RightKey", rightKeyParam);
                    await AddFolderInternal(folder);
                    await _dbContext.Database.ExecuteSqlCommandAsync("exec dbo." + TableName + "PostCreateNestedSetsNode @RightKey", rightKeyParam);
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
            }
        }
        public async Task AddFolder(NestedSetsFileSystemEntity folder, Guid parentFolderId)
        {
            var parentFolder = _dbSet.FirstOrDefault(f => f.Id == parentFolderId && f.Type == EntityType.Folder);
            if (parentFolder != null)
            {
                await AddFolder(folder, parentFolder);
            }
            throw new ArgumentException(string.Format("Folder with id = {0} not found in {1} table.", parentFolderId, TableName));
        }

        protected abstract Task UpdateFolderInternal(NestedSetsFileSystemEntity folder);
        public async Task UpdatFolder(NestedSetsFileSystemEntity folder)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    await UpdateFolderInternal(folder);
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
            }
        }

        protected abstract Task MoveFolderInternal();
        public async Task MoveFolder(NestedSetsFileSystemEntity folder, NestedSetsFileSystemEntity newFolder)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    await MoveFolderInternal();
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
            }
        }
        public async Task MoveFolder(NestedSetsFileSystemEntity folder, Guid newFolderId)
        {
            var newFolder = _dbSet.FirstOrDefault(f => f.Id == newFolderId && f.Type == EntityType.Folder);
            if (newFolder != null)
            {
                await MoveFolder(folder, newFolder);
            }
            throw new ArgumentException(string.Format("Folder with id = {0} not found in {1} table.", newFolderId, TableName));
        }

        protected abstract Task RemoveFolderInternal(NestedSetsFileSystemEntity folder);
        public async Task RemoveFolder(NestedSetsFileSystemEntity folder)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    var leftKeyParam = new SqlParameter { ParameterName = "LeftKey", Value = folder.LeftKey };
                    var rightKeyParam = new SqlParameter { ParameterName = "RightKey", Value = folder.RightKey };
                    await _dbContext.Database.ExecuteSqlCommandAsync("exec dbo." + TableName + "PostRemoveNestedSetsNode @LeftKey, @RightKey", leftKeyParam, rightKeyParam);
                    await RemoveFolderInternal(folder);
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
            }
        }

        //relations
        public async Task<ICollection<NestedSetsFileSystemEntity>> GetChildNodes(Guid id)
        {
            var node = _dbSet.FirstOrDefault(f => f.Id == id);
            if (node != null)
            {
                return await GetChildNodes(node);
            }
            throw new ArgumentException(string.Format("Node with id = {0} not found in {1} table.", id, TableName));
        }

        public async Task<ICollection<NestedSetsFileSystemEntity>> GetChildNodes(NestedSetsFileSystemEntity entity)
        {
            throw new NotImplementedException();
        }

        public async Task<NestedSetsFileSystemEntity> GetRootNode(Guid id)
        {
            var node = _dbSet.FirstOrDefault(f => f.Id == id);
            if (node != null)
            {
                return await GetRootNode(node);
            }
            throw new ArgumentException(string.Format("Node with id = {0} not found in {1} table.", id, TableName));
        }

        public async Task<NestedSetsFileSystemEntity> GetRootNode(NestedSetsFileSystemEntity entity)
        {
            throw new NotImplementedException();
        }

        public async Task<ICollection<NestedSetsFileSystemEntity>> GetRootNodes(Guid id)
        {
            var node = _dbSet.FirstOrDefault(f => f.Id == id);
            if (node != null)
            {
                return await GetRootNodes(node);
            }
            throw new ArgumentException(string.Format("Node with id = {0} not found in {1} table.", id, TableName));
        }

        public async Task<ICollection<NestedSetsFileSystemEntity>> GetRootNodes(NestedSetsFileSystemEntity entity)
        {
            throw new NotImplementedException();
        }

        #region Protected methods

        protected async Task ExecuteInsertSqlCommand<NS>(NS instance) where NS : NestedSetsFileSystemEntity
        {
            var columns = new List<string>();
            var parameters = new List<string>();
            var sqlParameters = new List<SqlParameter>();

            var sourceType = instance.GetType();
            var resPropertiesInfo = sourceType.GetProperties();
            foreach (var prop in resPropertiesInfo)
            {
                var sourcePropInfo = sourceType.GetProperty(prop.Name);
                var value = sourcePropInfo.GetValue(instance, null);
                if (value != null)
                {
                    columns.Add(prop.Name);
                    parameters.Add("@" + prop.Name);
                    sqlParameters.Add(new SqlParameter { ParameterName = prop.Name, Value = value });
                }
            }

            var commandText = string.Format(@"INSERT INTO {0} ({1}) VALUES ({2})",
                TableName, string.Join(",", columns), string.Join(",", parameters));
            await _dbContext.Database.ExecuteSqlCommandAsync(commandText, sqlParameters.ToArray());
        }

        protected async Task ExecuteUpdateSqlCommand<NS>(NS instance) where NS : NestedSetsFileSystemEntity
        {
            var expressions = new List<string>();
            var sqlParameters = new List<SqlParameter>();

            var sourceType = instance.GetType();
            var resPropertiesInfo = sourceType.GetProperties();
            foreach (var prop in resPropertiesInfo)
            {
                var sourcePropInfo = sourceType.GetProperty(prop.Name);
                var value = sourcePropInfo.GetValue(instance, null);
                if (value != null)
                {
                    expressions.Add(prop.Name + "= @" + prop.Name);
                    sqlParameters.Add(new SqlParameter { ParameterName = prop.Name, Value = value });
                }
            }

            var commandText = string.Format(@"UPDATE {0} SET {1} WHERE Id='{2}'",
                TableName, string.Join(",", expressions), instance.Id.ToString());
            await _dbContext.Database.ExecuteSqlCommandAsync(commandText, sqlParameters.ToArray());
        }

        protected async Task ExecuteDeleteSqlCommand<NS>(NS instance) where NS : NestedSetsFileSystemEntity
        {
            var commandText = string.Format(@"DELETE FROM {0} WHERE Id='{1}'",
                TableName, instance.Id.ToString());
            await _dbContext.Database.ExecuteSqlCommandAsync(commandText);
        }

        #endregion
    }
}