using OpenDataStorageCore.Entities.NestedSets;
using System.Data.Entity;

namespace OpenDataStorage.Common.DbContext.NestedSets
{
    public class ObjectTypeDbSetManager : BaseFSNestedSetsDbSetManager<ObjectType>
    {
        public ObjectTypeDbSetManager(DbSet<ObjectType> entities, Database database)
            : base(entities, database)
        {
            TableName = "ObjectTypes";
        }
    }
}