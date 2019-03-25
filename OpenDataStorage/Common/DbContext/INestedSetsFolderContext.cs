using OpenDataStorageCore;
using System;
using System.Threading.Tasks;

namespace OpenDataStorage.Common.DbContext
{
    public interface INestedSetsFolderContext
    {
        Task AddFolder(NestedSetsFolder folder, NestedSetsFolder parentFolder);

        Task AddFolder(NestedSetsFolder folder, Guid parentFolderId);

        Task RemoveFolder(NestedSetsFolder folder);

        Task MoveFolder(NestedSetsFolder folder, NestedSetsFolder newFolder);

        Task MoveFolder(NestedSetsFolder folder, Guid newFolderId);
    }
}
