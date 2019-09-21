using OpenDataStorageCore.Entities.NestedSets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace OpenDataStorage.Common.DbContext.Managers.NestedSetsManagers.Core
{
    public interface INestedSetsDbSetManager<T> where T : NestedSetsEntity
    {
        string TableName { get; }

        IQueryable<T> Entities { get; }

        Task<Guid> Create(T entity, Guid parentId);

        Task Update(T entity);

        Task Move(Guid entityId, Guid parentId);

        Task Delete(T entity);

        Task<T> GetNode(Guid id, params Expression<Func<T, object>>[] includedPath);

        Task<ICollection<T>> GetTree(params Expression<Func<T, object>>[] includedPath);

        Task<ICollection<T>> GetChildNodes(Guid id, bool includeItself = false, params Expression<Func<T, object>>[] includedPath);

        Task<T> GetParentNode(Guid id, params Expression<Func<T, object>>[] includedPath);

        Task<ICollection<T>> GetParentNodes(Guid id, bool includeItself = false, params Expression<Func<T, object>>[] includedPath);
    }
}
