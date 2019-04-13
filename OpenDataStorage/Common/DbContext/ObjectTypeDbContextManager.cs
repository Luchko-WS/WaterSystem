using OpenDataStorageCore;
using System.Data.Entity;

namespace OpenDataStorage.Common.DbContext
{
    public class ObjectTypeDbContextManager : BaseFSNestedSetsDbContextManager<ObjectType>
    {
        public ObjectTypeDbContextManager(DbSet<ObjectType> entities, Database database)
            : base(entities, database)
        {
            TableName = "Types";
        }
    }
}