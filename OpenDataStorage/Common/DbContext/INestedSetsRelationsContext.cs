using OpenDataStorageCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OpenDataStorage.Common.DbContext
{
    public interface INestedSetsRelationsContext
    {
        Task<string> GetPath(Guid id);

        Task<string> GetPath(NestedSetsEntity entity);

        Task<NestedSetsEntity> GetRootdNode(Guid id);

        Task<NestedSetsEntity> GetRootdNode(NestedSetsEntity entity);

        Task<ICollection<NestedSetsEntity>> GetRootNodes(Guid id);

        Task<ICollection<NestedSetsEntity>> GetRootNodes(NestedSetsEntity entity);

        Task<ICollection<NestedSetsEntity>> GetChildNodes(Guid id);

        Task<ICollection<NestedSetsEntity>> GetChildNodes(NestedSetsEntity entity);
    }
}
