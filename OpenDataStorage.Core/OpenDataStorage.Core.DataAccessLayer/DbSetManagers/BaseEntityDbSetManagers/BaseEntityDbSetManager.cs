using OpenDataStorage.Core.DataAccessLayer.DbContext;
using OpenDataStorage.Core.DataAccessLayer.DbSetManagers;
using OpenDataStorage.Core.Attributes;
using OpenDataStorage.Core.Entities;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace OpenDataStorage.Common
{
    public abstract class BaseEntityDbSetManager<T> : BaseDbSetManager<T> where T : BaseEntity
    {
        protected readonly IDbContextBase _dbContext;

        public BaseEntityDbSetManager(DbSet<T> dbSet, IDbContextBase dbContext, string tableName)
            : base(dbSet, tableName)
        {
            _dbContext = dbContext;
        }

        public override async Task<Guid> CreateAsync(T entity)
        {
            entity.Id = Guid.NewGuid();

            _dbSet.Add(entity);
            await SaveChangesAsync();

            return entity.Id;
        }

        public override async Task UpdateAsync(T entity)
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
            await SaveChangesAsync();
        }

        public override async Task DeleteAsync(Guid id)
        {
            var entity = await _dbSet.FirstOrDefaultAsync(e => e.Id == id);
            if (entity == null)
            {
                throw new ArgumentException(string.Format("Entity with id = {0} not found in {1} table.", id, _tableName));
            }
            _dbSet.Remove(entity);
            await SaveChangesAsync();
        }

        protected async Task SaveChangesAsync()
        {
            await _dbContext.SaveDbChangesAsync();
        }
    }
}