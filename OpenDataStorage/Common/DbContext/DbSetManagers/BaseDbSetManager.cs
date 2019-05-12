using OpenDataStorageCore;
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

        public virtual IQueryable<T> GetAllQuery()
        {
            return _dbSet.AsNoTracking();
        }

        public virtual IQueryable<T> GetEntityQuery(Guid id)
        {
            return _dbSet.AsNoTracking().Where(e => e.Id == id);
        }

        public virtual async Task Create(T entity)
        {
            _dbSet.Add(entity);
            await SaveChanges();
        }

        public virtual async Task Update(T entity)
        {
            var model = await _dbSet.FirstOrDefaultAsync(e => e.Id == entity.Id);
            if (model == null)
            {
                throw new ArgumentException(string.Format("Entity with id = {0} not found in {1} table.", entity.Id, TableName));
            }
            Mapper.MapProperties(entity, model);
            await SaveChanges();
        }

        public async Task Delete(Guid id)
        {
            var model = await _dbSet.FirstOrDefaultAsync(e => e.Id == id);
            if (model == null)
            {
                throw new ArgumentException(string.Format("Entity with id = {0} not found in {1} table.", id, TableName));
            }
            _dbSet.Remove(model);
            await SaveChanges();
        }

        abstract protected Task SaveChanges();
    }
}