using OpenDataStorage.Common.DbContext.Managers.DbSetManagers.Aliases;
using OpenDataStorage.Common.DbContext.Managers.DbSetManagers.CharacteristicValues;
using OpenDataStorage.Common.DbContext.Managers.NestedSetsManagers.Core;
using OpenDataStorageCore.Entities.Aliases;
using OpenDataStorageCore.Entities.NestedSets;
using System.Data.Entity;
using System.Threading.Tasks;

namespace OpenDataStorage.Common.DbContext
{
    public interface IApplicationDbContext : IApplicationDbContextBase
    {
        INestedSetsDbSetManager<HierarchyObject> HierarchyObjectContext { get; }

        INestedSetsDbSetManager<Characteristic> CharacteristicContext { get; }

        INestedSetsDbSetManager<ObjectType> ObjectTypeContext { get; }

        ICharacteristicValueDbSetManager CharacteristicValueDbSetManager { get; }

        IAliasDbSetManager<CharacteristicAlias> CharacteristicAliasDbSetManager { get; }

        IAliasDbSetManager<HierarchyObjectAlias> HierarchyObjectAliasDbSetManager { get; }

        Task ReloadEntityFromDb(object entity);

        DbContextTransaction BeginTransaction();
    }
}
