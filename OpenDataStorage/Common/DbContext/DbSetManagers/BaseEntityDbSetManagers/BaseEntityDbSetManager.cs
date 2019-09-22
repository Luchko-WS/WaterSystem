using OpenDataStorageCore.Attributes;
using OpenDataStorageCore.Entities;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace OpenDataStorage.Common.DbContext.DbSetManagers.BaseEntityDbSetManagers
{
    public abstract class BaseEntityDbSetManager<T> : BaseDbSetManager<T> where T : BaseEntity
    {
        protected readonly IApplicationDbContextBase _dbContext;

        public BaseEntityDbSetManager(DbSet<T> dbSet, IApplicationDbContextBase dbContext, string tableName)
            : base(dbSet, tableName)
        {
            _dbContext = dbContext;
        }

        public override async Task<Guid> Create(T entity)
        {
            entity.Id = Guid.NewGuid();

            _dbSet.Add(entity);
            await SaveChanges();

            return entity.Id;
        }

        public override async Task Update(T entity)
        {
            var dbEntity = await _dbSet.FirstOrDefaultAsync(e => e.Id == entity.Id);
            if (dbEntity == null)
            {
                throw new ArgumentException(string.Format("Entity with id = {0} not found in {1} table.", entity.Id, _tableName));
            }
            Mapper.MapProperties(entity, dbEntity, (prop) =>
            {
                var value = prop.GetValue(entity);
                var invalid = ((value is BaseEntity) || prop.GetCustomAttributes(typeof(IgnoreWhenUpdateAttribute), true).Any()
                    || (value == null && !(value is Nullable)));
                return !invalid;
            });
            await SaveChanges();
        }

        public override async Task Delete(Guid id)
        {
            var entity = await _dbSet.FirstOrDefaultAsync(e => e.Id == id);
            if (entity == null)
            {
                throw new ArgumentException(string.Format("Entity with id = {0} not found in {1} table.", id, _tableName));
            }
            _dbSet.Remove(entity);
            await SaveChanges();
        }

        protected async Task SaveChanges()
        {
            await _dbContext.SaveDbChangesAsync();
        }
    }
}