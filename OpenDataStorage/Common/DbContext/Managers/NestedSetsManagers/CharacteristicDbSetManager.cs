using OpenDataStorage.Common.DbContext.Managers.NestedSetsManagers.Core;
using OpenDataStorageCore.Entities.NestedSets;
using System.Data.Entity;

namespace OpenDataStorage.Common.DbContext.Managers.NestedSetsManagers
{
    public class CharacteristicDbSetManager : ExtendedFSNestedSetsDbSetManaget<Characteristic>
    {
        public CharacteristicDbSetManager(DbSet<Characteristic> entities, IDbContainer dbContainer)
            : base(entities, dbContainer, "Characteristics") { }
    }
}