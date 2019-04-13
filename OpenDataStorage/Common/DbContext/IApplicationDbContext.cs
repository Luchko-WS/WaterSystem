using OpenDataStorageCore;
using System.Threading.Tasks;

namespace OpenDataStorage.Common.DbContext
{
    public interface IApplicationDbContext
    {
        INestedSetsObjectContext<HierarchyObject> HierarchyObjectContext { get; }

        INestedSetsFSContext<Characteristic> CharacteristicObjectContext { get; }

        INestedSetsFSContext<ObjectType> ObjectTypeContext { get; }

        Task SaveDbChangesAsync();
    }
}
