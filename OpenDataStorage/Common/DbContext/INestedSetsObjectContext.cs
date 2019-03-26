using OpenDataStorageCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace OpenDataStorage.Common.DbContext
{
    public interface INestedSetsObjectContext<T>  : INestedSetsFolderContext, INestedSetsRelationsContext where T : NestedSetsObject
    {
        IQueryable<T> Entities { get; }

        Task AddObject(T @object, NestedSetsFileSystemEntity parentFolder);

        Task AddObject(T @object, Guid folderId);

        Task MoveObject(T @object, NestedSetsFileSystemEntity newFolder);

        Task MoveObject(T @object, Guid newFolderId);

        Task RemoveObject(T @object);
    }
}
