using OpenDataStorageCore.Attributes;
using OpenDataStorageCore.Entities;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace OpenDataStorage.Common.DbContext.DbSetManagers
{
    public abstract class BaseDbSetManager<T> : IDbSetManager<T> where T : BaseEntity
    {
        protected readonly DbSet<T> _dbSet;

        public string TableName { get; protected set; }

        public BaseDbSetManager(DbSet<T> dbSet)
        {
            _dbSet = dbSet;
        }

        public virtual IQueryable<T> GetAllQuery(bool includeAll = true)
        {
            var query = _dbSet.AsNoTracking();
            return includeAll ? IncludeDependencies(query) : query;
        }

        public virtual IQueryable<T> GetEntityQuery(Guid id, bool includeAll = true)
        {
            var query = _dbSet.AsNoTracking().Where(e => e.Id == id);
            return includeAll ? IncludeDependencies(query) : query;
        }

        public virtual async Task Create(T entity)
        {
            _dbSet.Add(entity);
            await SaveChanges();
        }

        public virtual async Task Update(T entity)
        {
            var dbEntity = await _dbSet.FirstOrDefaultAsync(e => e.Id == entity.Id);
            if (dbEntity == null)
            {
                throw new ArgumentException(string.Format("Entity with id = {0} not found in {1} table.", entity.Id, TableName));
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

        public virtual async Task Delete(Guid id)
        {
            var entity = await _dbSet.FirstOrDefaultAsync(e => e.Id == id);
            if (entity == null)
            {
                throw new ArgumentException(string.Format("Entity with id = {0} not found in {1} table.", id, TableName));
            }
            _dbSet.Remove(entity);
            await SaveChanges();
        }

        abstract protected Task SaveChanges();

        abstract protected IQueryable<T> IncludeDependencies(IQueryable<T> query);
    }
}