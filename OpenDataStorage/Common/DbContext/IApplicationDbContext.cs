using OpenDataStorage.Common.DbContext.DbSetManagers;
using OpenDataStorage.Common.DbContext.NestedSets;
using OpenDataStorageCore;
using System.Threading.Tasks;

namespace OpenDataStorage.Common.DbContext
{
    public interface IApplicationDbContext
    {
        INestedSetsObjectContext<HierarchyObject> HierarchyObjectContext { get; }

        INestedSetsFSContext<Characteristic> CharacteristicContext { get; }

        INestedSetsFSContext<ObjectType> ObjectTypeContext { get; }

        ICharacteristicValueDbSetManager CharacteristicValueDbSetManager { get; }

        Task SaveDbChangesAsync();
    }
}
