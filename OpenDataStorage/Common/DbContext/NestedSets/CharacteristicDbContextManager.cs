using OpenDataStorageCore.Entities.NestedSets;
using System.Data.Entity;

namespace OpenDataStorage.Common.DbContext.NestedSets
{
    public class CharacteristicDbSetManager : ExtendedFSNestedSetsDbSetManaget<Characteristic>
    {
        public CharacteristicDbSetManager(DbSet<Characteristic> entities, Database database)
            : base(entities, database)
        {
            TableName = "Characteristics";
        }
    }
}