using OpenDataStorageCore.Entities.NestedSets;
using System;
using System.Data.Entity;
using System.Threading.Tasks;

namespace OpenDataStorage.Common.DbContext.NestedSets
{
    public abstract class BaseFSNestedSetsDbSetManager<T> : BaseNestedSetsDbSetManager<T>, INestedSetsFSContext<T> where T : NestedSetsFSEntity
    {
        public BaseFSNestedSetsDbSetManager(DbSet<T> dbSet, IDbContainer dbContainer)
            : base(dbSet, dbContainer) { }

        public override async Task<Guid> Add(T entity, Guid parentId)
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
            return await AddInternal(entity, parentNode);
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
            await MoveInternal(entity, parentNode);
        }
    }
}