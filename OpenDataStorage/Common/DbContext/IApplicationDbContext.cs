using OpenDataStorageCore;
using System.Threading.Tasks;

namespace OpenDataStorage.Common.DbContext
{
    public interface IApplicationDbContext
    {
        INestedSetsObjectContext<HierarchyObject> HierarchyObjectContext { get; }

        INestedSetsObjectContext<Characteristic> CharacteristicObjectContext { get; }

        Task SaveDbChangesAsync();
    }
}
