using OpenDataStorageCore.Entities;
using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace OpenDataStorage.Core.DataAccessLayer.DbSetManagers
{
    public abstract class BaseDbSetManager<T> : IDbSetManager<T> where T : BaseEntity
    {
        protected readonly DbSet<T> _dbSet;
        protected readonly string _tableName;

        public string TableName => this._tableName;

        public IQueryable<T> Entities => this._dbSet;

        public BaseDbSetManager(DbSet<T> dbSet, string tableName)
        {
            _dbSet = dbSet;
            _tableName = tableName;
        }

        public virtual IQueryable<T> GetQuery(Guid id, params Expression<Func<T, object>>[] includedPath)
        {
            var query = _dbSet.AsNoTracking().Where(e => e.Id == id);
            return AggregateQuery(query, includedPath);
        }

        public virtual IQueryable<T> GetQueryWithAllDependencies(Guid id)
        {
            var query = _dbSet.AsNoTracking().Where(e => e.Id == id);
            return IncludeAllDependencies(query);
        }

        public virtual IQueryable<T> GetAllQuery(params Expression<Func<T, object>>[] includedPath)
        {
            var query = _dbSet.AsNoTracking();
            return AggregateQuery(query, includedPath);
        }

        public virtual IQueryable<T> GetAllQueryWithAllDependencies()
        {
            var query = _dbSet.AsNoTracking();
            return IncludeAllDependencies(query);
        }

        public abstract Task<Guid> Create(T entity);

        public abstract Task Update(T entity);

        public abstract Task Delete(Guid id);

        protected abstract IQueryable<T> IncludeAllDependencies(IQueryable<T> query);

        protected IQueryable<T> AggregateQuery(IQueryable<T> query, Expression<Func<T, object>>[] includedPath)
        {
            return includedPath.Aggregate(query, (current, p) => current.Include(p));
        }
    }
}