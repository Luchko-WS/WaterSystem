using OpenDataStorageCore;
using System;
using System.Threading.Tasks;

namespace OpenDataStorage.Common.DbContext
{
    public interface INestedSetsFSContext<T> : INestedSetsObjectContext<T> where T : NestedSetsFSEntity
    {
        Task AddFolder(NestedSetsFSEntity folder, Guid parentFolderId);

        Task UpdatFolder(NestedSetsFSEntity folder);

        Task RemoveFolder(NestedSetsFSEntity folder);

        Task MoveFolder(NestedSetsFSEntity folder, Guid newFolderId);
    }
}
