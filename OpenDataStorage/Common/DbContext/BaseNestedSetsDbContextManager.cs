using OpenDataStorageCore;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace OpenDataStorage.Common.DbContext
{
    public abstract class BaseNestedSetsDbContextManager<T> : INestedSetsObjectContext<T> where T : NestedSetsEntity
    {
        protected DbSet<T> _dbSet;
        protected Database _database;

        public string TableName { get; protected set; }

        public BaseNestedSetsDbContextManager(DbSet<T> dbSet, Database database)
        {
            _database = database;
            _dbSet = dbSet;
        }

        public IQueryable<T> Entities => this._dbSet;

        public virtual async Task Add(T entity, Guid parentId)
        {
            var parentNode = _dbSet.FirstOrDefault(f => f.Id == parentId);
            if (parentNode == null)
            {
                throw new ArgumentException(string.Format("Node with id = {0} not found in {1} table.", parentId, TableName));
            }
            await AddInternal(entity, parentNode);
        }
        protected async Task AddInternal(T entity, NestedSetsEntity parentNode)
        {
            entity.LeftKey = parentNode.RightKey;
            entity.RightKey = parentNode.RightKey + 1;
            entity.Level = parentNode.Level + 1;

            using (var transaction = _database.BeginTransaction())
            {
                try
                {
                    await ExecutePreInsertSqlCommand(parentNode);
                    await ExecuteInsertSqlCommand(entity);
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
            }
        }

        public virtual async Task Update(T entity)
        {
            await UpdateInternal(entity);
        }
        protected async Task UpdateInternal(T entity)
        {
            using (var transaction = _database.BeginTransaction())
            {
                try
                {
                    await ExecuteUpdateSqlCommand(entity);
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
            }
        }

        public virtual async Task Move(Guid id, Guid parentId)
        {
            var entity = _dbSet.FirstOrDefault(f => f.Id == id);
            if (entity == null)
            {
                throw new ArgumentException(string.Format("Node with id = {0} not found in {1} table.", id, TableName));
            }
            var parentEntity = _dbSet.FirstOrDefault(f => f.Id == parentId);
            if (parentEntity == null)
            {
                throw new ArgumentException(string.Format("Node with id = {0} not found in {1} table.", parentId, TableName));
            }
            await MoveInternal(entity, parentEntity);
        }
        protected async Task MoveInternal(T entity, NestedSetsEntity parentEntity)
        {
            if (parentEntity.LeftKey > entity.LeftKey && parentEntity.RightKey < entity.RightKey)
            {
                throw new ArgumentException(string.Format("Could move node {0} in child node {1} in {2} table.", entity.Id, parentEntity.Id, TableName));
            }

            using (var transaction = _database.BeginTransaction())
            {
                try
                {
                    await ExecuteMoveSqlCommand(entity, parentEntity);
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
            }
        }

        public virtual async Task Remove(T entity)
        {
            await RemoveInternal(entity);
        }
        protected async Task RemoveInternal(T entity)
        {
            using (var transaction = _database.BeginTransaction())
            {
                try
                {
                    await ExecuteDeleteSqlCommand(entity);
                    await ExecutePostDeleteSqlCommand(entity);
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
            }
        }

        #region Nested Sets Relation
        public virtual async Task<T> GetNode(Guid id, params Expression<Func<T, object>>[] includedPath)
        {
            var query = GetNodeQuery(id);
            query = includedPath.Aggregate(query, (current, p) => current.Include(p));
            
            var node = await query.FirstOrDefaultAsync();
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
            var query = GetTreeQuery();
            query = includedPath.Aggregate(query, (current, p) => current.Include(p));
            return await query.ToListAsync();
        }
        private IQueryable<T> GetTreeQuery()
        {
            return _dbSet.OrderBy(n => n.LeftKey);
        }

        public virtual async Task<ICollection<T>> GetChildNodes(Guid id, bool includeItself = false, params Expression<Func<T, object>>[] includedPath)
        {
            var node = _dbSet.FirstOrDefault(f => f.Id == id);
            if (node == null)
            {
                throw new ArgumentException(string.Format("Node with id = {0} not found in {1} table.", id, TableName));
            }
            return await GetChildNodes(node, includeItself, includedPath);
        }
        private async Task<ICollection<T>> GetChildNodes(T entity, bool includeItself, params Expression<Func<T, object>>[] includedPath)
        {
            return await GetChildNodesQuery(entity, includeItself).ToListAsync();
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
            var node = _dbSet.FirstOrDefault(f => f.Id == id);
            if (node == null)
            {
                throw new ArgumentException(string.Format("Node with id = {0} not found in {1} table.", id, TableName));
            }
            return await GetParentNode(node, includedPath);
        }
        private async Task<T> GetParentNode(T entity, params Expression<Func<T, object>>[] includedPath)
        {
            return await GetParentNodeQuery(entity).FirstOrDefaultAsync();
        }
        private IQueryable<T> GetParentNodeQuery(T entity)
        {
            return _dbSet.Where(e => e.LeftKey <= entity.LeftKey && e.RightKey >= entity.RightKey && e.Level == entity.Level - 1);
        }

        public virtual async Task<ICollection<T>> GetParentNodes(Guid id, bool includeItself = false, params Expression<Func<T, object>>[] includedPath)
        {
            var node = _dbSet.FirstOrDefault(f => f.Id == id);
            if (node == null)
            {
                throw new ArgumentException(string.Format("Node with id = {0} not found in {1} table.", id, TableName));
            }
            return await GetParentNodes(node, includeItself, includedPath);
        }
        private async Task<ICollection<T>> GetParentNodes(T entity, bool includeItself, params Expression<Func<T, object>>[] includedPath)
        {
            return await GetRootNodesQuery(entity, includeItself).ToListAsync();
        }
        private IQueryable<T> GetRootNodesQuery(T entity, bool includeItself)
        {
            return _dbSet
                .Where(e =>
                    (e.LeftKey < entity.LeftKey && e.RightKey > entity.RightKey && e.Level < entity.Level)
                    || (includeItself && e.Id == entity.Id))
                .OrderBy(e => e.LeftKey);
        }

        #endregion

        #region Protected methods

        private async Task ExecutePreInsertSqlCommand<NS>(NS instance) where NS : NestedSetsEntity
        {
            var rightKeyParam = new SqlParameter { ParameterName = "RightKey", Value = instance.RightKey };
            await _database.ExecuteSqlCommandAsync("exec dbo." + TableName + "PreCreateNestedSetsNode @RightKey", rightKeyParam);
        }

        private async Task ExecuteInsertSqlCommand<NS>(NS instance) where NS : NestedSetsEntity
        {
            var columns = new List<string>();
            var parameters = new List<string>();
            var sqlParameters = new List<SqlParameter>();

            var sourceType = instance.GetType();
            var resPropertiesInfo = sourceType.GetProperties();
            foreach (var prop in resPropertiesInfo)
            {
                var sourcePropInfo = sourceType.GetProperty(prop.Name);
                var value = sourcePropInfo.GetValue(instance, null);
                if (!(value is BaseEntity) && (value != null && !(value is Nullable) || value == null && (value is Nullable)))
                {
                    columns.Add(prop.Name);
                    parameters.Add("@" + prop.Name);
                    sqlParameters.Add(new SqlParameter { ParameterName = prop.Name, Value = value });
                }
            }

            var commandText = string.Format(@"INSERT INTO {0} ({1}) VALUES ({2})",
                TableName, string.Join(",", columns), string.Join(",", parameters));
            await _database.ExecuteSqlCommandAsync(commandText, sqlParameters.ToArray());
        }

        private async Task ExecuteUpdateSqlCommand<NS>(NS instance) where NS : NestedSetsEntity
        {
            var expressions = new List<string>();
            var sqlParameters = new List<SqlParameter>();

            var sourceType = instance.GetType();
            var resPropertiesInfo = sourceType.GetProperties();
            foreach (var prop in resPropertiesInfo)
            {
                var sourcePropInfo = sourceType.GetProperty(prop.Name);
                var value = sourcePropInfo.GetValue(instance, null);
                if (!(value is BaseEntity) && (value != null && !(value is Nullable) || value == null && (value is Nullable)))
                {
                    expressions.Add(prop.Name + "= @" + prop.Name);
                    sqlParameters.Add(new SqlParameter { ParameterName = prop.Name, Value = value });
                }
            }

            var commandText = string.Format(@"UPDATE {0} SET {1} WHERE Id='{2}'",
                TableName, string.Join(",", expressions), instance.Id.ToString());
            await _database.ExecuteSqlCommandAsync(commandText, sqlParameters.ToArray());
        }

        private async Task ExecuteMoveSqlCommand<NS>(NS entity, NS parent) where NS: NestedSetsEntity
        {
            var nodeLeftKeyParam = new SqlParameter { ParameterName = "NodeLeftKey", Value = entity.LeftKey };
            var nodeRightKeyParam = new SqlParameter { ParameterName = "NodeRightKey", Value = entity.RightKey };
            var nodeLevelParam = new SqlParameter { ParameterName = "NodeLevel", Value = entity.Level };

            var parentLeftKeyParam = new SqlParameter { ParameterName = "ParentLeftKey", Value = parent.LeftKey };
            var parentRightKeyParam = new SqlParameter { ParameterName = "ParentRightKey", Value = parent.RightKey };
            var parentLevelParam = new SqlParameter { ParameterName = "ParentLevel", Value = parent.Level };

            await _database.ExecuteSqlCommandAsync("exec dbo." + TableName + "MoveNestedSetsNode @NodeLeftKey, @NodeRightKey, @NodeLevel, @ParentLeftKey, @ParentRightKey, @ParentLevel", 
                nodeLeftKeyParam, nodeRightKeyParam, nodeLevelParam, parentLeftKeyParam, parentRightKeyParam, parentLevelParam);
        }

        private async Task ExecuteDeleteSqlCommand<NS>(NS instance) where NS : NestedSetsEntity
        {
            if (instance.Level == 0) throw new ArgumentException("Cannot delete the root node!");
            var commandText = string.Format(@"DELETE FROM {0} WHERE LeftKey >= {1} AND RightKey <= {2}",
                TableName, instance.LeftKey, instance.RightKey);
            await _database.ExecuteSqlCommandAsync(commandText);
        }

        private async Task ExecutePostDeleteSqlCommand<NS>(NS instance) where NS : NestedSetsEntity
        {
            var leftKeyParam = new SqlParameter { ParameterName = "LeftKey", Value = instance.LeftKey };
            var rightKeyParam = new SqlParameter { ParameterName = "RightKey", Value = instance.RightKey };
            await _database.ExecuteSqlCommandAsync("exec dbo." + TableName + "PostRemoveNestedSetsNode @LeftKey, @RightKey", leftKeyParam, rightKeyParam);
        }

        #endregion
    }
}