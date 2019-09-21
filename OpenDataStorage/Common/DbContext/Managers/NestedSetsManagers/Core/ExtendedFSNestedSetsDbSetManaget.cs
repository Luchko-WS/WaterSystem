using OpenDataStorageCore.Entities.NestedSets;
using System;
using System.Data.Entity;
using System.Threading.Tasks;

namespace OpenDataStorage.Common.DbContext.Managers.NestedSetsManagers.Core
{
    public abstract class ExtendedFSNestedSetsDbSetManaget<T> : FSNestedSetsDbSetManager<T> where T : NestedSetsFSEntity
    {
        public ExtendedFSNestedSetsDbSetManaget(DbSet<T> entities, IDbContainer dbContainer, string tableName) 
            : base(entities, dbContainer, tableName) { }

        public override async Task<Guid> Create(T entity, Guid parentId)
        {
            var parentNode = await _dbSet.FirstOrDefaultAsync(f => f.Id == parentId);
            if (parentNode == null)
            {
                throw new ArgumentException(string.Format("Node with id = {0} not found in {1} table.", parentId, TableName));
            }
            if (entity.EntityType == EntityType.Folder && parentNode.EntityType == EntityType.File)
            {
                throw new ArgumentException(string.Format("Cannot create child folder in entity {0} in {1} table.", parentId, TableName));
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
            if (entity.EntityType == EntityType.Folder && parentNode.EntityType == EntityType.File)
            {
                throw new ArgumentException(string.Format("Cannot create child folder in entity {0} in {1} table.", parentId, TableName));
            }
            await ExecuteMove(entity, parentNode);
        }
    }
}