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
            var tableNameParam = new SqlParameter { ParameterName = "TableName", Value = TableName };
            var rightKeyParam = new SqlParameter { ParameterName = "RightKey", Value = parentFolder.RightKey };

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    await _dbContext.Database.ExecuteSqlCommandAsync("exec dbo.PreCreateNestedSetsNode @TableName, @RightKey", tableNameParam, rightKeyParam);
                    await AddObjectInternal(@object);
                    await _dbContext.Database.ExecuteSqlCommandAsync("exec dbo.PostCreateNestedSetsNode @TableName, @RightKey", tableNameParam, rightKeyParam);
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
                    var tableNameParam = new SqlParameter { ParameterName = "TableName", Value = TableName };
                    var leftKeyParam = new SqlParameter { ParameterName = "LeftKey", Value = @object.LeftKey };
                    var rightKeyParam = new SqlParameter { ParameterName = "RightKey", Value = @object.RightKey };
                    await _dbContext.Database.ExecuteSqlCommandAsync("exec dbo.PostRemoveNestedSetsNode @TableName, @LeftKey, @RightKey", tableNameParam, leftKeyParam, rightKeyParam);
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
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    var tableNameParam = new SqlParameter { ParameterName = "TableName", Value = TableName };
                    var rightKeyParam = new SqlParameter { ParameterName = "RightKey", Value = parentFolder.RightKey };
                    await _dbContext.Database.ExecuteSqlCommandAsync("exec dbo.PreCreateNestedSetsNode @TableName, @RightKey", tableNameParam, rightKeyParam);
                    await AddFolderInternal(folder);
                    await _dbContext.Database.ExecuteSqlCommandAsync("exec dbo.PostCreateNestedSetsNode @TableName, @RightKey", tableNameParam, rightKeyParam);
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
                    var tableNameParam = new SqlParameter { ParameterName = "TableName", Value = TableName };
                    var leftKeyParam = new SqlParameter { ParameterName = "LeftKey", Value = folder.LeftKey };
                    var rightKeyParam = new SqlParameter { ParameterName = "RightKey", Value = folder.RightKey };
                    await _dbContext.Database.ExecuteSqlCommandAsync("exec dbo.PostRemoveNestedSetsNode @TableName, @LeftKey, @RightKey", tableNameParam, leftKeyParam, rightKeyParam);
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
        public Task<ICollection<NestedSetsEntity>> GetChildNodes(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<NestedSetsEntity>> GetChildNodes(NestedSetsEntity entity)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetPath(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetPath(NestedSetsEntity entity)
        {
            throw new NotImplementedException();
        }

        public Task<NestedSetsEntity> GetRootdNode(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<NestedSetsEntity> GetRootdNode(NestedSetsEntity entity)
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<NestedSetsEntity>> GetRootNodes(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<NestedSetsEntity>> GetRootNodes(NestedSetsEntity entity)
        {
            throw new NotImplementedException();
        }
    }
}