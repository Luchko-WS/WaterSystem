using OpenDataStorageCore;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace OpenDataStorage.Common.DbContext
{
    public abstract class BaseNestedSetsDbContextManager<T> : INestedSetsObjectContext<T>, INestedSetsFolderContext, INestedSetsRelationsContext where T : NestedSetsObject
    {
        protected DbSet<T> _dbSet;

        public string TableName { get; protected set; }

        public BaseNestedSetsDbContextManager(DbSet<T> dbSet)
        {
            _dbSet = dbSet;
        }

        protected abstract Task PreAdd(T @object, NestedSetsFileSystemEntity parentFolder);
        protected abstract Task PostAdd(T @object, NestedSetsFileSystemEntity parentFolder);

        public async Task AddObject(T @object, NestedSetsFileSystemEntity parentFolder)
        {
            await PreAdd(@object, parentFolder);
            @object.Type = EntityType.File;
            _dbSet.Add(@object);
            await PostAdd(@object, parentFolder);
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

        protected abstract Task PreMove();
        protected abstract Task PostMove();

        public async Task MoveObject(T @object, NestedSetsFileSystemEntity newFolder)
        {
            await PreMove();
            
            await PostMove();
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

        protected abstract Task PreRemove(T @object);
        protected abstract Task PostRemove(T @object);

        public async Task RemoveObject(T @object)
        {
            await PreRemove(@object);
            _dbSet.Remove(@object);
            await PostRemove(@object);
        }

        //folder
        protected abstract Task PreAddFolder(NestedSetsFileSystemEntity folder, NestedSetsFileSystemEntity parentFolder);
        protected abstract Task PostAddFolder(NestedSetsFileSystemEntity folder, NestedSetsFileSystemEntity parentFolder);

        public async Task AddFolder(NestedSetsFileSystemEntity folder, NestedSetsFileSystemEntity parentFolder)
        {
            await PreAddFolder(folder, parentFolder);
            folder.Type = EntityType.Folder;
            _dbSet.Add((T)folder);
            await PostAddFolder(folder, parentFolder);
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

        protected abstract Task PreMoveFolder();
        protected abstract Task PostMoveFolder();

        public async Task MoveFolder(NestedSetsFileSystemEntity folder, NestedSetsFileSystemEntity newFolder)
        {
            await PreMoveFolder();
            await PostMoveFolder();
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

        protected abstract Task PreRemoveFolder(NestedSetsFileSystemEntity folder);
        protected abstract Task PostRemoveFolder(NestedSetsFileSystemEntity folder);

        public async Task RemoveFolder(NestedSetsFileSystemEntity folder)
        {
            await PreRemoveFolder(folder);
            _dbSet.Remove((T)folder);
            await PostRemoveFolder(folder);
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