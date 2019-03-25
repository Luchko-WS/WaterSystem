using OpenDataStorageCore;
using System;
using System.Threading.Tasks;

namespace OpenDataStorage.Common.DbContext
{
    public interface INestedSetsFolderContext
    {
        Task AddFolder(NestedSetsFileSystemEntity folder, NestedSetsFileSystemEntity parentFolder);

        Task AddFolder(NestedSetsFileSystemEntity folder, Guid parentFolderId);

        Task RemoveFolder(NestedSetsFileSystemEntity folder);

        Task MoveFolder(NestedSetsFileSystemEntity folder, NestedSetsFileSystemEntity newFolder);

        Task MoveFolder(NestedSetsFileSystemEntity folder, Guid newFolderId);
    }
}
