using OpenDataStorageCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace OpenDataStorage.Common.DbContext
{
    public interface INestedSetsObjectContext<T>  : INestedSetsFolderContext, INestedSetsEntityRelationContext<T> where T : NestedSetsObject
    {
        IQueryable<T> Entities { get; }

        string TableName { get; }

        Task AddObject(T @object, Guid folderId);

        Task UpdateObject(T @object);

        Task MoveObject(T @object, Guid newFolderId);

        Task RemoveObject(Guid objectId);
    }
}
