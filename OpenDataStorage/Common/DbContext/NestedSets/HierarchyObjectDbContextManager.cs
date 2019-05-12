using OpenDataStorageCore;
using System.Data.Entity;

namespace OpenDataStorage.Common.DbContext.NestedSets
{
    public class HierarchyObjectDbContextManager : BaseNestedSetsDbSetManager<HierarchyObject>
    {
        public HierarchyObjectDbContextManager(DbSet<HierarchyObject> entities, Database database)
            : base(entities, database)
        {
            TableName = "HierarchyObjects";
        }
    }
}