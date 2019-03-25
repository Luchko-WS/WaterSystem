using OpenDataStorageCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace OpenDataStorage.Common.DbContext
{
    public interface IApplicationDbContext
    {
        IQueryable<NestedSetsEntity> HierarchyObjects { get; }

        IQueryable<NestedSetsEntity> Characteristics { get; }

        Task SaveDbChangesAsync();

        Task CreateCharacteristicsFolder(NestedSetsFolder folder, Guid parentFolderId);

        Task CreateCharacteristics(Characteristic characteristic, Guid parentFolderId);
    }
}
