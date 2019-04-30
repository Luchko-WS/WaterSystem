using OpenDataStorageCore;
using System.Data.Entity;

namespace OpenDataStorage.Common.DbContext
{
    public class HierarchyObjectDbContextManager : BaseNestedSetsDbContextManager<HierarchyObject>
    {
        public HierarchyObjectDbContextManager(DbSet<HierarchyObject> entities, Database database)
            : base(entities, database)
        {
            TableName = "HierarchyObjects";
        }
    }
}