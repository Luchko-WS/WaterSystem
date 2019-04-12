using OpenDataStorageCore;
using System;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace OpenDataStorage.Common.DbContext
{
    public abstract class BaseFSNestedSetsDbContextManager<T> : BaseNestedSetsDbContextManager<T>, INestedSetsFSContext<T> where T : NestedSetsFSEntity
    {
        public BaseFSNestedSetsDbContextManager(DbSet<T> dbSet, Database database)
            : base(dbSet, database) { }

        public async Task AddFolder(NestedSetsFSEntity folder, Guid parentFolderId)
        {
            var parentFolder = _dbSet.FirstOrDefault(f => f.Id == parentFolderId && f.Type == EntityType.Folder);
            if (parentFolder == null)
            {
                throw new ArgumentException(string.Format("Folder with id = {0} not found in {1} table.", parentFolderId, TableName));
            }
            await AddFolderInternal(folder, parentFolder);
        }
        protected virtual async Task AddFolderInternal(NestedSetsFSEntity folder, NestedSetsFSEntity parentFolder)
        {
            folder.LeftKey = parentFolder.RightKey;
            folder.RightKey = parentFolder.RightKey + 1;
            folder.Level = parentFolder.Level + 1;

            using (var transaction = _database.BeginTransaction())
            {
                try
                {
                    await ExecutePreInsertSqlCommand(parentFolder.RightKey);
                    await ExecuteInsertSqlCommand(folder);
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
            }
        }

        public async Task UpdatFolder(NestedSetsFSEntity folder)
        {
            if (folder.Type != EntityType.Folder)
            {
                throw new ArgumentException(string.Format("Folder with id = {0} not found in {1} table.", folder.Id, TableName));
            }
            await UpdateFolderInternal(folder);
        }
        protected virtual async Task UpdateFolderInternal(NestedSetsFSEntity folder)
        {
            using (var transaction = _database.BeginTransaction())
            {
                try
                {
                    await ExecuteUpdateSqlCommand(folder);
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
            }
        }

        public async Task MoveFolder(NestedSetsFSEntity folder, Guid newFolderId)
        {
            var newFolder = _dbSet.FirstOrDefault(f => f.Id == newFolderId && f.Type == EntityType.Folder);
            if (newFolder == null)
            {
                throw new ArgumentException(string.Format("Folder with id = {0} not found in {1} table.", newFolderId, TableName));
            }
            await MoveFolderInternal(folder, newFolder);
        }
        private async Task MoveFolderInternal(NestedSetsFSEntity folder, NestedSetsFSEntity newFolder)
        {
            using (var transaction = _database.BeginTransaction())
            {
                try
                {
                    throw new NotImplementedException();
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
            }
        }

        public async Task RemoveFolder(Guid folderId)
        {
            var folder = _dbSet.FirstOrDefault(o => o.Id == folderId && o.Type == EntityType.Folder);
            if (folder == null)
            {
                throw new ArgumentException(string.Format("Folder with id = {0} not found in {1} table.", folder, TableName));
            }
            await RemoveFolderInternal(folder);
        }
        protected virtual async Task RemoveFolderInternal(NestedSetsFSEntity folder)
        {
            using (var transaction = _database.BeginTransaction())
            {
                try
                {
                    await ExecuteDeleteSqlCommand(folder);
                    await ExecutePostDeleteSqlCommand(folder.LeftKey, folder.RightKey);
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
            }
        }

        #region Object Context

        protected override Task AddObjectInternal(T @object, NestedSetsEntity parentNode)
        {
            var folder = parentNode as NestedSetsFSEntity;
            if (folder != null && folder.Type == EntityType.Folder)
            {
                return base.AddObjectInternal(@object, parentNode);
            }
            throw new ArgumentException(string.Format("Folder with id = {0} not found in {1} table.", parentNode.Id, TableName));
        }

        protected override Task MoveObjectInternal(T @object, NestedSetsEntity newParentNode)
        {
            var folder = newParentNode as NestedSetsFSEntity;
            if (folder != null && folder.Type == EntityType.Folder)
            {
                return base.MoveObjectInternal(@object, newParentNode);
            }
            throw new ArgumentException(string.Format("Folder with id = {0} not found in {1} table.", newParentNode.Id, TableName));
        }

        #endregion
    }
}