using OpenDataStorageCore.Entities.NestedSets;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace OpenDataStorage.Core.DataAccessLayer.DbSetManagers.NestedSetsEntityManagers.Core
{
    public interface INestedSetsDbSetManager<T> : IDbSetManager<T> where T : NestedSetsEntity
    {
        Task<Guid> Create(T entity, Guid parentId);

        Task Move(Guid entityId, Guid parentId);

        Task<ICollection<T>> GetChildren(Guid id, bool includeItself = false, params Expression<Func<T, object>>[] includedPath);

        Task<ICollection<T>> GetChildrenWithAllDependencies(Guid id, bool includeItself = false);

        Task<T> GetParent(Guid id, params Expression<Func<T, object>>[] includedPath);

        Task<T> GetParentWithAllDependencies(Guid id);

        Task<ICollection<T>> GetParents(Guid id, bool includeItself = false, params Expression<Func<T, object>>[] includedPath);

        Task<ICollection<T>> GetParentsWithAllDependencies(Guid id, bool includeItself = false);
    }
}
