using OpenDataStorageCore;
using System.Data.Entity;

namespace OpenDataStorage.Common.DbContext
{
    public class CharacteristicDbContextManager : BaseFSNestedSetsDbContextManager<Characteristic>
    {
        public CharacteristicDbContextManager(DbSet<Characteristic> entities, Database database)
            : base(entities, database)
        {
            TableName = "Characteristics";
        }
    }
}