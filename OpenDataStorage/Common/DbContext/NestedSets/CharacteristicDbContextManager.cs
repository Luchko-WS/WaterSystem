using OpenDataStorageCore;
using System.Data.Entity;

namespace OpenDataStorage.Common.DbContext.NestedSets
{
    public class CharacteristicDbSetManager : BaseFSNestedSetsDbSetManager<Characteristic>
    {
        public CharacteristicDbSetManager(DbSet<Characteristic> entities, Database database)
            : base(entities, database)
        {
            TableName = "Characteristics";
        }
    }
}