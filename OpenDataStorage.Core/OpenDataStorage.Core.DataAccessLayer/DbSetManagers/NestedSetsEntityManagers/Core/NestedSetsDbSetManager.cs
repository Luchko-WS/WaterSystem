using OpenDataStorage.Core.DataAccessLayer.DbContext;
using OpenDataStorage.Core.Entities.NestedSets;
using System;
using System.Data.Entity;
using System.Threading.Tasks;

namespace OpenDataStorage.Core.DataAccessLayer.DbSetManagers.NestedSetsEntityManagers.Core
{
    public abstract class NestedSetsDbSetManager<T> : BaseNestedSetsEntityDbManager<T> where T : NestedSetsEntity
    {
        public NestedSetsDbSetManager(DbSet<T> dbSet, IDbContainer dbContainer, string tableName)
            : base(dbSet, dbContainer, tableName) { }

        public override Task<Guid> CreateAsync(T entity)
        {
            throw new NotImplementedException("Use CreateAsync(T entity, Guid parentId) instead");
        }

        public override async Task<Guid> CreateAsync(T entity, Guid parentId)
        {
            var parentNode = await CheckAndGetEntityByIdAsync(parentId);
            return await ExecuteCreateAsync(entity, parentNode);
        }

        public override async Task UpdateAsync(T entity)
        {
            await ExecuteUpdateAsync(entity);
        }

        public override async Task MoveAsync(Guid id, Guid parentId)
        {
            var entity = await CheckAndGetEntityByIdAsync(id);
            var parentEntity = await CheckAndGetEntityByIdAsync(parentId);
            await ExecuteMoveAsync(entity, parentEntity);
        }

        public override async Task DeleteAsync(Guid id)
        {
            var entity = await CheckAndGetEntityByIdAsync(id);
            await ExecuteDeleteAsync(entity);
        }
    }
}