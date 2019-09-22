using OpenDataStorage.Core.DataAccessLayer.DbContext;
using OpenDataStorageCore.Entities.NestedSets;
using System;
using System.Data.Entity;
using System.Threading.Tasks;

namespace OpenDataStorage.Core.DataAccessLayer.DbSetManagers.NestedSetsEntityManagers.Core
{
    public abstract class FSNestedSetsDbSetManager<T> : NestedSetsDbSetManager<T> where T : NestedSetsFSEntity
    {
        public FSNestedSetsDbSetManager(DbSet<T> dbSet, IDbContainer dbContainer, string tableName)
            : base(dbSet, dbContainer, tableName) { }

        public override async Task<Guid> Create(T entity, Guid parentId)
        {
            var parentNode = await CheckAndGetEntityByIdAsync(parentId);
            if (parentNode.EntityType != EntityType.Folder)
            {
                throw new ArgumentException(string.Format("Entity with id = {0} is not a folder in {1} table.", parentId, TableName));
            }
            return await ExecuteCreateAsync(entity, parentNode);
        }

        public override async Task Move(Guid id, Guid parentId)
        {
            var entity = await CheckAndGetEntityByIdAsync(id);
            var parentNode = await CheckAndGetEntityByIdAsync(parentId);
            if (parentNode.EntityType != EntityType.Folder)
            {
                throw new ArgumentException(string.Format("Node with id = {0} is not a folder in {1} table.", parentId, TableName));
            }
            await ExecuteMoveAsync(entity, parentNode);
        }
    }
}