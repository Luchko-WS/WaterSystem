using OpenDataStorageCore;
using System.Data.Entity;

namespace OpenDataStorage.Common.DbContext
{
    public class HierarchyObjectTypeDbContextManager : BaseFSNestedSetsDbContextManager<HierarchyObjectType>
    {
        public HierarchyObjectTypeDbContextManager(DbSet<HierarchyObjectType> entities, Database database)
            : base(entities, database)
        {
            TableName = "Types";
        }
    }
}