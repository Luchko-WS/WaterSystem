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
        protected DbSet<NestedSetsEntity> _dbSet;

        public string TableName { get; protected set; }

        public BaseNestedSetsDbContextManager(DbSet<NestedSetsEntity> dbSet)
        {
            _dbSet = dbSet;
        }

        protected abstract Task PreAdd(T @object, NestedSetsFolder parentFolder);
        protected abstract Task PostAdd(T @object, NestedSetsFolder parentFolder);

        public async Task AddObject(T @object, NestedSetsFolder parentFolder)
        {
            await PreAdd(@object, parentFolder);
            _dbSet.Add(@object);
            await PostAdd(@object, parentFolder);
        }

        public async Task AddObject(T @object, Guid folderId)
        {
            var folder = _dbSet.FirstOrDefault(f => f.Id == folderId && f is NestedSetsFolder);
            if (folder != null)
            {
                await AddObject(@object, (NestedSetsFolder)folder);
            }
            throw new ArgumentException(string.Format("Folder with id = {0} not found in {1} table.", folderId, TableName));
        }

        protected abstract Task PreMove();
        protected abstract Task PostMove();

        public async Task MoveObject(T @object, NestedSetsFolder newFolder)
        {
            await PreMove();
            
            await PostMove();
        }

        public async Task MoveObject(T @bject, Guid newFolderId)
        {
            var folder = _dbSet.FirstOrDefault(f => f.Id == newFolderId && f is NestedSetsFolder);
            if (folder != null)
            {
                await MoveObject(@bject, (NestedSetsFolder)folder);
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
        protected abstract Task PreAddFolder(NestedSetsFolder folder, NestedSetsFolder parentFolder);
        protected abstract Task PostAddFolder(NestedSetsFolder folder, NestedSetsFolder parentFolder);

        public async Task AddFolder(NestedSetsFolder folder, NestedSetsFolder parentFolder)
        {
            await PreAddFolder(folder, parentFolder);
            _dbSet.Add(folder);
            await PostAddFolder(folder, parentFolder);
        }

        public async Task AddFolder(NestedSetsFolder folder, Guid parentFolderId)
        {
            var parentFolder = _dbSet.FirstOrDefault(f => f.Id == parentFolderId && f is NestedSetsFolder);
            if (folder != null)
            {
                await AddFolder(folder, (NestedSetsFolder)parentFolder);
            }
            throw new ArgumentException(string.Format("Folder with id = {0} not found in {1} table.", parentFolderId, TableName));

        }

        protected abstract Task PreMoveFolder();
        protected abstract Task PostMoveFolder();

        public async Task MoveFolder(NestedSetsFolder folder, NestedSetsFolder newFolder)
        {
            await PreMoveFolder();
            await PostMoveFolder();
        }

        public async Task MoveFolder(NestedSetsFolder folder, Guid newFolderId)
        {
            var newFolder = _dbSet.FirstOrDefault(f => f.Id == newFolderId && f is NestedSetsFolder);
            if (folder != null)
            {
                await MoveFolder(folder, (NestedSetsFolder)folder);
            }
            throw new ArgumentException(string.Format("Folder with id = {0} not found in {1} table.", newFolderId, TableName));
        }

        protected abstract Task PreRemoveFolder(NestedSetsFolder folder);
        protected abstract Task PostRemoveFolder(NestedSetsFolder folder);

        public async Task RemoveFolder(NestedSetsFolder folder)
        {
            await PreRemoveFolder(folder);
            _dbSet.Remove(folder);
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