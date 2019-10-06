using OpenDataStorage.Core.DataAccessLayer.DbSetManagers.BaseEntityDbSetManagers.Aliases;
using OpenDataStorage.Core.DataAccessLayer.DbSetManagers.BaseEntityDbSetManagers.CharacteristicValues;
using OpenDataStorage.Core.DataAccessLayer.DbSetManagers.NestedSetsEntityManagers.Core;
using OpenDataStorage.Core.Entities.Aliases;
using OpenDataStorage.Core.Entities.NestedSets;
using System.Data.Entity;
using System.Threading.Tasks;

namespace OpenDataStorage.Core.DataAccessLayer.DbContext
{
    public interface IOpenDataStorageDbContext : IDbContextBase
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
