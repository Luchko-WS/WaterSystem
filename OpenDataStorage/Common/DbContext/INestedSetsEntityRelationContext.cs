using OpenDataStorageCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OpenDataStorage.Common.DbContext
{
    public interface INestedSetsEntityRelationContext<T> where T : NestedSetsFileSystemEntity
    {
        Task<T> GetRootNode(Guid id);

        Task<ICollection<T>> GetRootNodes(Guid id);

        Task<ICollection<T>> GetChildNodes(Guid id);

        Task<ICollection<T>> GetTree();
    }
}
