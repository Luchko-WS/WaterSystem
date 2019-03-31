using OpenDataStorageCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OpenDataStorage.Common.DbContext
{
    public interface INestedSetsFileSystemContext
    {
        Task<NestedSetsFileSystemEntity> GetRootNode(Guid id);

        Task<NestedSetsFileSystemEntity> GetRootNode(NestedSetsFileSystemEntity entity);

        Task<ICollection<NestedSetsFileSystemEntity>> GetRootNodes(Guid id);

        Task<ICollection<NestedSetsFileSystemEntity>> GetRootNodes(NestedSetsFileSystemEntity entity);

        Task<ICollection<NestedSetsFileSystemEntity>> GetChildNodes(Guid id);

        Task<ICollection<NestedSetsFileSystemEntity>> GetChildNodes(NestedSetsFileSystemEntity entity);
    }
}
