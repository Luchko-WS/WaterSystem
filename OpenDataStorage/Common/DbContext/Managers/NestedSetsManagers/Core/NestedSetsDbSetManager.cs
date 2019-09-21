using OpenDataStorageCore.Entities.NestedSets;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace OpenDataStorage.Common.DbContext.Managers.NestedSetsManagers.Core
{
    public abstract class NestedSetsDbSetManager<T> : BaseSqlNestedSetsDbManager<T>, INestedSetsDbSetManager<T> where T : NestedSetsEntity
    {
        protected readonly DbSet<T> _dbSet;

        public NestedSetsDbSetManager(DbSet<T> dbSet, IDbContainer dbContainer, string tableName)
            : base(dbContainer, tableName)
        {
            _dbSet = dbSet;
        }

        public string TableName => this._tableName;

        public IQueryable<T> Entities => this._dbSet;

        public virtual async Task<Guid> Create(T entity, Guid parentId)
        {
            var parentNode = await _dbSet.FirstOrDefaultAsync(f => f.Id == parentId);
            if (parentNode == null)
            {
                throw new ArgumentException(string.Format("Node with id = {0} not found in {1} table.", parentId, TableName));
            }
            return await ExecuteCreate(entity, parentNode);
        }

        public virtual async Task Update(T entity)
        {
            await ExecuteUpdate(entity);
        }

        public virtual async Task Move(Guid id, Guid parentId)
        {
            var entity = await _dbSet.FirstOrDefaultAsync(f => f.Id == id);
            if (entity == null)
            {
                throw new ArgumentException(string.Format("Node with id = {0} not found in {1} table.", id, TableName));
            }
            var parentEntity = await _dbSet.FirstOrDefaultAsync(f => f.Id == parentId);
            if (parentEntity == null)
            {
                throw new ArgumentException(string.Format("Node with id = {0} not found in {1} table.", parentId, TableName));
            }
            await ExecuteMove(entity, parentEntity);
        }

        public virtual async Task Delete(T entity)
        {
            await ExecuteDelete(entity);
        }

        public virtual async Task<T> GetNode(Guid id, params Expression<Func<T, object>>[] includedPath)
        {
            var node = await AggregateQuery(GetNodeQuery(id), includedPath).FirstOrDefaultAsync();
            if (node == null)
            {
                throw new ArgumentException(string.Format("Node with id = {0} not found in {1} table.", id, TableName));
            }
            return node;
        }
        private IQueryable<T> GetNodeQuery(Guid id)
        {
            return _dbSet.Where(e => e.Id == id);
        }

        public virtual async Task<ICollection<T>> GetTree(params Expression<Func<T, object>>[] includedPath)
        {
            return await AggregateQuery(GetTreeQuery(), includedPath).ToListAsync();
        }
        private IQueryable<T> GetTreeQuery()
        {
            return _dbSet.OrderBy(n => n.LeftKey);
        }

        public virtual async Task<ICollection<T>> GetChildNodes(Guid id, bool includeItself = false, params Expression<Func<T, object>>[] includedPath)
        {
            var entity = await _dbSet.FirstOrDefaultAsync(f => f.Id == id);
            if (entity == null)
            {
                throw new ArgumentException(string.Format("Node with id = {0} not found in {1} table.", id, TableName));
            }
            return await AggregateQuery(GetChildNodesQuery(entity, includeItself), includedPath).ToListAsync();
        }
        private IQueryable<T> GetChildNodesQuery(T entity, bool includeItself)
        {
            return _dbSet
               .Where(e => e.LeftKey >= entity.LeftKey && e.RightKey <= entity.RightKey
                || (includeItself && e.Id == entity.Id))
               .OrderBy(n => n.LeftKey);
        }

        public virtual async Task<T> GetParentNode(Guid id, params Expression<Func<T, object>>[] includedPath)
        {
            var entity = await _dbSet.FirstOrDefaultAsync(f => f.Id == id);
            if (entity == null)
            {
                throw new ArgumentException(string.Format("Node with id = {0} not found in {1} table.", id, TableName));
            }
            return await AggregateQuery(GetParentNodeQuery(entity), includedPath).FirstOrDefaultAsync();
        }
        private IQueryable<T> GetParentNodeQuery(T entity)
        {
            return _dbSet.Where(e => e.LeftKey <= entity.LeftKey && e.RightKey >= entity.RightKey && e.Level == entity.Level - 1);
        }

        public virtual async Task<ICollection<T>> GetParentNodes(Guid id, bool includeItself = false, params Expression<Func<T, object>>[] includedPath)
        {
            var entity = await _dbSet.FirstOrDefaultAsync(f => f.Id == id);
            if (entity == null)
            {
                throw new ArgumentException(string.Format("Node with id = {0} not found in {1} table.", id, TableName));
            }
            return await AggregateQuery(GetRootNodesQuery(entity, includeItself), includedPath).ToListAsync();
        }
        private IQueryable<T> GetRootNodesQuery(T entity, bool includeItself)
        {
            return _dbSet
                .Where(e =>
                    (e.LeftKey < entity.LeftKey && e.RightKey > entity.RightKey && e.Level < entity.Level)
                    || (includeItself && e.Id == entity.Id))
                .OrderBy(e => e.LeftKey);
        }

        private IQueryable<T> AggregateQuery(IQueryable<T> query, Expression<Func<T, object>>[] includedPath)
        {
            return includedPath.Aggregate(query, (current, p) => current.Include(p));
        }
    }
}