using OpenDataStorageCore;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
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

        public async Task AddObject(T @object, Guid parentId)
        {
            var parentNode = _dbSet.FirstOrDefault(f => f.Id == parentId);
            if (parentNode == null)
            {
                throw new ArgumentException(string.Format("Node with id = {0} not found in {1} table.", parentId, TableName));
            }
            await AddObjectInternal(@object, parentNode);
        }
        protected virtual async Task AddObjectInternal(T @object, NestedSetsEntity parentNode)
        {
            @object.LeftKey = parentNode.RightKey;
            @object.RightKey = parentNode.RightKey + 1;
            @object.Level = parentNode.Level + 1;

            using (var transaction = _database.BeginTransaction())
            {
                try
                {
                    await ExecutePreInsertSqlCommand(parentNode.RightKey);
                    await ExecuteInsertSqlCommand(@object);
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
            }
        }

        public async Task UpdateObject(T @object)
        {
            await UpdateObjectInternal(@object);
        }
        protected virtual async Task UpdateObjectInternal(T @object)
        {
            using (var transaction = _database.BeginTransaction())
            {
                try
                {
                    await ExecuteUpdateSqlCommand(@object);
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
            }
        }

        public async Task MoveObject(T @bject, Guid newParentId)
        {
            var parentNode = _dbSet.FirstOrDefault(f => f.Id == newParentId);
            if (parentNode == null)
            {
                throw new ArgumentException(string.Format("Node with id = {0} not found in {1} table.", newParentId, TableName));
            }
            await MoveObjectInternal(@bject, parentNode);
        }
        protected virtual async Task MoveObjectInternal(T @object, NestedSetsEntity newParentNode)
        {
            using (var transaction = _database.BeginTransaction())
            {
                try
                {
                    throw new NotImplementedException();
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
            }
        }

        public async Task RemoveObject(T @object)
        {
            await RemoveObjectInternal(@object);
        }
        protected virtual async Task RemoveObjectInternal(T @object)
        {
            using (var transaction = _database.BeginTransaction())
            {
                try
                {
                    await ExecuteDeleteSqlCommand(@object);
                    await ExecutePostDeleteSqlCommand(@object.LeftKey, @object.RightKey);
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
        public virtual async Task<T> GetNode(Guid id)
        {
            var node = await GetNodeQuery(id).FirstOrDefaultAsync();
            if(node == null)
            {
                throw new ArgumentException(string.Format("Node with id = {0} not found in {1} table.", id, TableName));
            }
            return node;
        }
        private IQueryable<T> GetNodeQuery(Guid id)
        {
            return _dbSet.Where(e => e.Id == id);
        }

        public virtual async Task<ICollection<T>> GetTree()
        {
            return await GetTreeQuery().ToListAsync();
        }
        private IQueryable<T> GetTreeQuery()
        {
            return _dbSet.OrderBy(n => n.LeftKey);
        }

        public virtual async Task<ICollection<T>> GetChildNodes(Guid id)
        {
            var node = _dbSet.FirstOrDefault(f => f.Id == id);
            if (node == null)
            {
                throw new ArgumentException(string.Format("Node with id = {0} not found in {1} table.", id, TableName));
            }
            return await GetChildNodes(node);
        }
        private async Task<ICollection<T>> GetChildNodes(T entity)
        {
            return await GetChildNodesQuery(entity).ToListAsync();
        }
        private IQueryable<T> GetChildNodesQuery(T entity)
        {
            return _dbSet
               .Where(e => e.LeftKey >= entity.LeftKey && e.RightKey <= entity.RightKey)
               .OrderBy(n => n.LeftKey);
        }
        
        public virtual async Task<T> GetParentNode(Guid id)
        {
            var node = _dbSet.FirstOrDefault(f => f.Id == id);
            if (node == null)
            {
                throw new ArgumentException(string.Format("Node with id = {0} not found in {1} table.", id, TableName));
            }
            return await GetParentNode(node);
        }
        private async Task<T> GetParentNode(T entity)
        {
            return await GetParentNodeQuery(entity).FirstOrDefaultAsync();
        }
        private IQueryable<T> GetParentNodeQuery(T entity)
        {
            return _dbSet.Where(e => e.LeftKey <= entity.LeftKey && e.RightKey >= entity.RightKey && e.Level == entity.Level - 1);
        }

        public virtual async Task<ICollection<T>> GetParentNodes(Guid id, bool includeItself = false)
        {
            var node = _dbSet.FirstOrDefault(f => f.Id == id);
            if (node == null)
            {
                throw new ArgumentException(string.Format("Node with id = {0} not found in {1} table.", id, TableName));
            }
            return await GetParentNodes(node, includeItself);
        }
        private async Task<ICollection<T>> GetParentNodes(T entity, bool includeItself)
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

        protected async Task ExecutePreInsertSqlCommand(int nodeRightKey)
        {
            var rightKeyParam = new SqlParameter { ParameterName = "RightKey", Value = nodeRightKey };
            await _database.ExecuteSqlCommandAsync("exec dbo." + TableName + "PreCreateNestedSetsNode @RightKey", rightKeyParam);
        }

        protected async Task ExecuteInsertSqlCommand<NS>(NS instance) where NS : NestedSetsEntity
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
                if (value != null)
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

        protected async Task ExecuteUpdateSqlCommand<NS>(NS instance) where NS : NestedSetsEntity
        {
            var expressions = new List<string>();
            var sqlParameters = new List<SqlParameter>();

            var sourceType = instance.GetType();
            var resPropertiesInfo = sourceType.GetProperties();
            foreach (var prop in resPropertiesInfo)
            {
                var sourcePropInfo = sourceType.GetProperty(prop.Name);
                var value = sourcePropInfo.GetValue(instance, null);
                if (value != null)
                {
                    expressions.Add(prop.Name + "= @" + prop.Name);
                    sqlParameters.Add(new SqlParameter { ParameterName = prop.Name, Value = value });
                }
            }

            var commandText = string.Format(@"UPDATE {0} SET {1} WHERE Id='{2}'",
                TableName, string.Join(",", expressions), instance.Id.ToString());
            await _database.ExecuteSqlCommandAsync(commandText, sqlParameters.ToArray());
        }

        protected async Task ExecuteDeleteSqlCommand<NS>(NS instance) where NS : NestedSetsEntity
        {
            if (instance.Level == 0) throw new ArgumentException("Cannot delete the root node!");
            var commandText = string.Format(@"DELETE FROM {0} WHERE LeftKey >= {1} AND RightKey <= {2}",
                TableName, instance.LeftKey, instance.RightKey);
            await _database.ExecuteSqlCommandAsync(commandText);
        }

        protected async Task ExecutePostDeleteSqlCommand(int nodeLeftKey, int nodeRightKey)
        {
            var leftKeyParam = new SqlParameter { ParameterName = "LeftKey", Value = nodeLeftKey };
            var rightKeyParam = new SqlParameter { ParameterName = "RightKey", Value = nodeRightKey };
            await _database.ExecuteSqlCommandAsync("exec dbo." + TableName + "PostRemoveNestedSetsNode @LeftKey, @RightKey", leftKeyParam, rightKeyParam);
        }

        #endregion
    }
}