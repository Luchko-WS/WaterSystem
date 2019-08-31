using OpenDataStorage.Common.DbContext.DbSetManagers;
using OpenDataStorage.Common.DbContext.DbSetManagers.Aliases;
using OpenDataStorage.Common.DbContext.NestedSets;
using OpenDataStorageCore.Entities.NestedSets;
using System.Data.Entity;
using System.Threading.Tasks;

namespace OpenDataStorage.Common.DbContext
{
    public interface IApplicationDbContext : IApplicationDbContextBase
    {
        INestedSetsObjectContext<HierarchyObject> HierarchyObjectContext { get; }

        INestedSetsFSContext<Characteristic> CharacteristicContext { get; }

        INestedSetsFSContext<ObjectType> ObjectTypeContext { get; }

        ICharacteristicValueDbSetManager CharacteristicValueDbSetManager { get; }

        ICharacteristicAliasDbSetManager CharacteristicAliasDbSetManager { get; }

        IHierarchyObjectAliasDbSetManager HierarchyObjectAliasDbSetManager { get; }

        Task ReloadEntityFromDb(object entity);

        DbContextTransaction BeginTransaction();
    }
}
