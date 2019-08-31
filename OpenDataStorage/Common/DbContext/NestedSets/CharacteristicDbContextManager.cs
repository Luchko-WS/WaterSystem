using OpenDataStorageCore.Entities.NestedSets;
using System.Data.Entity;

namespace OpenDataStorage.Common.DbContext.NestedSets
{
    public class CharacteristicDbSetManager : ExtendedFSNestedSetsDbSetManaget<Characteristic>
    {
        public CharacteristicDbSetManager(DbSet<Characteristic> entities, IDbContainer dbContainer)
            : base(entities, dbContainer)
        {
            TableName = "Characteristics";
        }
    }
}