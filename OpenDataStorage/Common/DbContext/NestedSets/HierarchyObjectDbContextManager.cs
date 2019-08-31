using OpenDataStorageCore.Entities.NestedSets;
using System.Data.Entity;

namespace OpenDataStorage.Common.DbContext.NestedSets
{
    public class HierarchyObjectDbContextManager : BaseNestedSetsDbSetManager<HierarchyObject>
    {
        public HierarchyObjectDbContextManager(DbSet<HierarchyObject> entities, IDbContainer dbContainer)
            : base(entities, dbContainer)
        {
            TableName = "HierarchyObjects";
        }
    }
}