using OpenDataStorageCore;
using System;
using System.Threading.Tasks;

namespace OpenDataStorage.Common.DbContext
{
    public interface INestedSetsObjectContext<T>  where T : NestedSetsObject
    {
        Task AddObject(T @object, NestedSetsFolder parentFolder);

        Task AddObject(T @object, Guid folderId);

        Task MoveObject(T @object, NestedSetsFolder newFolder);

        Task MoveObject(T @object, Guid newFolderId);

        Task RemoveObject(T @object);
    }
}
