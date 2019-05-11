using OpenDataStorageCore;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace OpenDataStorage.Common.DbContext
{
    public abstract class BaseFSNestedSetsDbContextManager<T> : BaseNestedSetsDbContextManager<T>, INestedSetsFSContext<T> where T : NestedSetsFSEntity
    {
        public BaseFSNestedSetsDbContextManager(DbSet<T> dbSet, Database database)
            : base(dbSet, database) { }

        public override async Task Add(T entity, Guid parentId)
        {
            var parentNode = _dbSet.FirstOrDefault(f => f.Id == parentId);
            if (parentNode == null)
            {
                throw new ArgumentException(string.Format("Node with id = {0} not found in {1} table.", parentId, TableName));
            }
            if (parentNode.EntityType != EntityType.Folder)
            {
                throw new ArgumentException(string.Format("Node with id = {0} is not a folder in {1} table.", parentId, TableName));
            }
            await AddInternal(entity, parentNode);
        }

        public override async Task Move(Guid id, Guid parentId)
        {
            var entity = _dbSet.FirstOrDefault(f => f.Id == id);
            if (entity == null)
            {
                throw new ArgumentException(string.Format("Node with id = {0} not found in {1} table.", id, TableName));
            }
            var parentNode = _dbSet.FirstOrDefault(f => f.Id == parentId);
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