using OpenDataStorageCore.Entities.NestedSets;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace OpenDataStorage.Common.DbContext.NestedSets
{
    public interface INestedSetsEntityRelationContext<T> where T : NestedSetsEntity
    {
        Task<T> GetNode(Guid id, params Expression<Func<T, object>>[] includedPath);

        Task<ICollection<T>> GetTree(params Expression<Func<T, object>>[] includedPath);

        Task<ICollection<T>> GetChildNodes(Guid id, bool includeItself = false, params Expression<Func<T, object>>[] includedPath);

        Task<T> GetParentNode(Guid id, params Expression<Func<T, object>>[] includedPath);

        Task<ICollection<T>> GetParentNodes(Guid id, bool includeItself = false, params Expression<Func<T, object>>[] includedPath);
    }
}
