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
        protected Database _database;

        public string TableName { get; protected set; }

        public BaseNestedSetsDbContextManager(DbSet<T> dbSet, Database database)
        {
            _database = database;
            _dbSet = dbSet;
        }

        public IQueryable<T> Entities => this._dbSet;

        protected abstract Task AddObjectInternal(T @object);
        public async Task AddObject(T @object, Guid folderId)
        {
            var folder = _dbSet.FirstOrDefault(f => f.Id == folderId && f.Type == EntityType.Folder);
            if (folder == null)
            {
                throw new ArgumentException(string.Format("Folder with id = {0} not found in {1} table.", folderId, TableName));
            }
            await AddObject(@object, folder);
        }
        private async Task AddObject(T @object, NestedSetsFileSystemEntity parentFolder)
        {
            @object.LeftKey = parentFolder.RightKey;
            @object.RightKey = parentFolder.RightKey + 1;
            @object.Level = parentFolder.Level + 1;

            using (var transaction = _database.BeginTransaction())
            {
                try
                {
                    var rightKeyParam = new SqlParameter { ParameterName = "RightKey", Value = parentFolder.RightKey };
                    await _database.ExecuteSqlCommandAsync("exec dbo." + TableName + "PreCreateNestedSetsNode @RightKey", rightKeyParam);
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

        protected abstract Task UpdateObjectInternal(T @object);
        public async Task UpdateObject(T @object)
        {
            using (var transaction = _database.BeginTransaction())
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
        public async Task MoveObject(T @bject, Guid newFolderId)
        {
            var folder = _dbSet.FirstOrDefault(f => f.Id == newFolderId && f.Type == EntityType.Folder);
            if (folder == null)
            {
                throw new ArgumentException(string.Format("Folder with id = {0} not found in {1} table.", newFolderId, TableName));
            }
            await MoveObject(@bject, folder);
        }
        private async Task MoveObject(T @object, NestedSetsFileSystemEntity newFolder)
        {
            using (var transaction = _database.BeginTransaction())
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

        protected abstract Task RemoveObjectInternal(T @object);
        public async Task RemoveObject(Guid objectId)
        {
            var @object = _dbSet.FirstOrDefault(f => f.Id == objectId && f.Type == EntityType.File);
            if (@object == null)
            {
                throw new ArgumentException(string.Format("Entity with id = {0} not found in {1} table.", objectId, TableName));
            }
            await RemoveObject(@object);
        }
        private async Task RemoveObject(T @object)
        {
            using (var transaction = _database.BeginTransaction())
            {
                try
                {
                    await RemoveObjectInternal(@object);
                    var leftKeyParam = new SqlParameter { ParameterName = "LeftKey", Value = @object.LeftKey };
                    var rightKeyParam = new SqlParameter { ParameterName = "RightKey", Value = @object.RightKey };
                    await _database.ExecuteSqlCommandAsync("exec dbo." + TableName + "PostRemoveNestedSetsNode @LeftKey, @RightKey", leftKeyParam, rightKeyParam);
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
        public async Task AddFolder(NestedSetsFileSystemEntity folder, Guid parentFolderId)
        {
            var parentFolder = _dbSet.FirstOrDefault(f => f.Id == parentFolderId && f.Type == EntityType.Folder);
            if (parentFolder == null)
            {
                throw new ArgumentException(string.Format("Folder with id = {0} not found in {1} table.", parentFolderId, TableName));
            }
            await AddFolder(folder, parentFolder);
        }
        private async Task AddFolder(NestedSetsFileSystemEntity folder, NestedSetsFileSystemEntity parentFolder)
        {
            folder.LeftKey = parentFolder.RightKey;
            folder.RightKey = parentFolder.RightKey + 1;
            folder.Level = parentFolder.Level + 1;

            using (var transaction = _database.BeginTransaction())
            {
                try
                {
                    var rightKeyParam = new SqlParameter { ParameterName = "RightKey", Value = parentFolder.RightKey };
                    await _database.ExecuteSqlCommandAsync("exec dbo." + TableName + "PreCreateNestedSetsNode @RightKey", rightKeyParam);
                    await AddFolderInternal(folder);
                    await _database.ExecuteSqlCommandAsync("exec dbo." + TableName + "PostCreateNestedSetsNode @RightKey", rightKeyParam);
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
            }
        }

        protected abstract Task UpdateFolderInternal(NestedSetsFileSystemEntity folder);
        public async Task UpdatFolder(NestedSetsFileSystemEntity folder)
        {
            using (var transaction = _database.BeginTransaction())
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
        public async Task MoveFolder(NestedSetsFileSystemEntity folder, Guid newFolderId)
        {
            var newFolder = _dbSet.FirstOrDefault(f => f.Id == newFolderId && f.Type == EntityType.Folder);
            if (newFolder == null)
            {
                throw new ArgumentException(string.Format("Folder with id = {0} not found in {1} table.", newFolderId, TableName));
            }
            await MoveFolder(folder, newFolder);
        }
        private async Task MoveFolder(NestedSetsFileSystemEntity folder, NestedSetsFileSystemEntity newFolder)
        {
            using (var transaction = _database.BeginTransaction())
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

        protected abstract Task RemoveFolderInternal(NestedSetsFileSystemEntity folder);
        public async Task RemoveFolder(Guid folderId)
        {
            var folder = _dbSet.FirstOrDefault(o => o.Id == folderId && o.Type == EntityType.Folder);
            if (folder == null)
            {
                throw new ArgumentException(string.Format("Folder with id = {0} not found in {1} table.", folder, TableName));
            }
            await RemoveFolder(folder);
        }
        private async Task RemoveFolder(NestedSetsFileSystemEntity folder)
        {
            using (var transaction = _database.BeginTransaction())
            {
                try
                {
                    var leftKeyParam = new SqlParameter { ParameterName = "LeftKey", Value = folder.LeftKey };
                    var rightKeyParam = new SqlParameter { ParameterName = "RightKey", Value = folder.RightKey };
                    await _database.ExecuteSqlCommandAsync("exec dbo." + TableName + "PostRemoveNestedSetsNode @LeftKey, @RightKey", leftKeyParam, rightKeyParam);
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
        public virtual async Task<ICollection<T>> GetChildNodes(Guid id)
        {
            var node = _dbSet.FirstOrDefault(f => f.Id == id);
            if (node == null)
            {
                throw new ArgumentException(string.Format("Node with id = {0} not found in {1} table.", id, TableName));
            }
            return await GetChildNodes(node);
        }
        private async Task<ICollection<T>> GetChildNodes(NestedSetsFileSystemEntity entity)
        {
            return await GetChildNodesQuery(entity).ToListAsync();
        }
        private IQueryable<T> GetChildNodesQuery(NestedSetsFileSystemEntity entity)
        {
            return _dbSet
               .Where(e => e.LeftKey >= entity.LeftKey && e.RightKey <= entity.RightKey)
               .OrderBy(n => n.LeftKey);
        }

        public virtual async Task<ICollection<T>> GetTree()
        {
            return await GetTreeQuery().ToListAsync();
        }
        private IQueryable<T> GetTreeQuery()
        {
            return _dbSet.OrderBy(n => n.LeftKey);
        }

        public virtual async Task<T> GetRootNode(Guid id)
        {
            var node = _dbSet.FirstOrDefault(f => f.Id == id);
            if (node == null)
            {
                throw new ArgumentException(string.Format("Node with id = {0} not found in {1} table.", id, TableName));
            }
            return await GetRootNode(node);
        }
        private async Task<T> GetRootNode(NestedSetsFileSystemEntity entity)
        {
            return await GetRootNodeQuery(entity).FirstOrDefaultAsync();
        }
        private IQueryable<T> GetRootNodeQuery(NestedSetsFileSystemEntity entity)
        {
            return _dbSet.Where(e => e.LeftKey <= entity.LeftKey && e.RightKey >= entity.RightKey && e.Level == entity.Level - 1);
        }

        public virtual async Task<ICollection<T>> GetRootNodes(Guid id)
        {
            var node = _dbSet.FirstOrDefault(f => f.Id == id);
            if (node == null)
            {
                throw new ArgumentException(string.Format("Node with id = {0} not found in {1} table.", id, TableName));
            }
            return await GetRootNodes(node);
        }
        private async Task<ICollection<T>> GetRootNodes(NestedSetsFileSystemEntity entity)
        {
            return await GetRootNodesQuery(entity).ToListAsync();
        }
        private IQueryable<T> GetRootNodesQuery(NestedSetsFileSystemEntity entity)
        {
            return _dbSet
                .Where(e => e.LeftKey <= entity.LeftKey && e.RightKey >= entity.RightKey)
                .OrderBy(e => e.LeftKey);
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
            await _database.ExecuteSqlCommandAsync(commandText, sqlParameters.ToArray());
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
            await _database.ExecuteSqlCommandAsync(commandText, sqlParameters.ToArray());
        }

        protected async Task ExecuteDeleteSqlCommand<NS>(NS instance) where NS : NestedSetsFileSystemEntity
        {
            var commandText = string.Format(@"DELETE FROM {0} WHERE LeftKey >= {1} AND RightKey <= {2}",
                TableName, instance.LeftKey, instance.RightKey);
            await _database.ExecuteSqlCommandAsync(commandText);
        }

        #endregion
    }
}