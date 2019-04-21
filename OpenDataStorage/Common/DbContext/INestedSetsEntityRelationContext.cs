using OpenDataStorageCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OpenDataStorage.Common.DbContext
{
    public interface INestedSetsEntityRelationContext<T> where T : NestedSetsEntity
    {
        Task<T> GetNode(Guid id);

        Task<ICollection<T>> GetTree();

        Task<ICollection<T>> GetChildNodes(Guid id);

        Task<T> GetParentNode(Guid id);

        Task<ICollection<T>> GetParentNodes(Guid id);
    }
}
