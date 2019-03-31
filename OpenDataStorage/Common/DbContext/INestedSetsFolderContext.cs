using OpenDataStorageCore;
using System;
using System.Threading.Tasks;

namespace OpenDataStorage.Common.DbContext
{
    public interface INestedSetsFolderContext
    {
        Task AddFolder(NestedSetsFileSystemEntity folder, Guid parentFolderId);

        Task UpdatFolder(NestedSetsFileSystemEntity folder);

        Task RemoveFolder(Guid folderId);

        Task MoveFolder(NestedSetsFileSystemEntity folder, Guid newFolderId);
    }
}
