using OpenDataStorageCore.Entities.NestedSets;
using System;
using System.Data.Entity;
using System.Threading.Tasks;

namespace OpenDataStorage.Common.DbContext.Managers.NestedSetsManagers.Core
{
    public abstract class FSNestedSetsDbSetManager<T> : NestedSetsDbSetManager<T> where T : NestedSetsFSEntity
    {
        public FSNestedSetsDbSetManager(DbSet<T> dbSet, IDbContainer dbContainer, string tableName)
            : base(dbSet, dbContainer, tableName) { }

        public override async Task<Guid> Create(T entity, Guid parentId)
        {
            var parentNode = await _dbSet.FirstOrDefaultAsync(f => f.Id == parentId);
            if (parentNode == null)
            {
                throw new ArgumentException(string.Format("Node with id = {0} not found in {1} table.", parentId, TableName));
            }
            if (parentNode.EntityType != EntityType.Folder)
            {
                throw new ArgumentException(string.Format("Node with id = {0} is not a folder in {1} table.", parentId, TableName));
            }
            return await ExecuteCreate(entity, parentNode);
        }

        public override async Task Move(Guid id, Guid parentId)
        {
            var entity = await _dbSet.FirstOrDefaultAsync(f => f.Id == id);
            if (entity == null)
            {
                throw new ArgumentException(string.Format("Node with id = {0} not found in {1} table.", id, TableName));
            }
            var parentNode = await _dbSet.FirstOrDefaultAsync(f => f.Id == parentId);
            if (parentNode == null)
            {
                throw new ArgumentException(string.Format("Node with id = {0} not found in {1} table.", parentId, TableName));
            }
            if (parentNode.EntityType != EntityType.Folder)
            {
                throw new ArgumentException(string.Format("Node with id = {0} is not a folder in {1} table.", parentId, TableName));
            }
            await ExecuteMove(entity, parentNode);
        }
    }
}