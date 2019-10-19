using OpenDataStorage.Core.Entities.NestedSets;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace OpenDataStorage.Core.DataAccessLayer.DbSetManagers.NestedSetsEntityManagers.Core
{
    public interface INestedSetsDbSetManager<T> : IDbSetManager<T> where T : NestedSetsEntity
    {
        Task<Guid> CreateAsync(T entity, Guid parentId);

        Task MoveAsync(Guid entityId, Guid parentId);

        Task<ICollection<T>> GetChildrenAsync(Guid id, bool includeItself = false,
            params Expression<Func<T, object>>[] includedPath);

        Task<ICollection<T>> GetChildrenWithAllDependenciesAsync(Guid id, bool includeItself = false);

        Task<T> GetParentAsync(Guid id, params Expression<Func<T, object>>[] includedPath);

        Task<T> GetParentWithAllDependenciesAsync(Guid id);

        Task<ICollection<T>> GetParentsAsync(Guid id, bool includeItself = false,
            params Expression<Func<T, object>>[] includedPath);

        Task<ICollection<T>> GetParentsWithAllDependenciesAsync(Guid id, bool includeItself = false);
    }
}
