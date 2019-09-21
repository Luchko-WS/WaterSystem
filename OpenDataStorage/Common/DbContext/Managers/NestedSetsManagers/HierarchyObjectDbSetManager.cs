using OpenDataStorage.Common.DbContext.Managers.NestedSetsManagers.Core;
using OpenDataStorageCore.Entities.NestedSets;
using System.Data.Entity;

namespace OpenDataStorage.Common.DbContext.Managers.NestedSetsManagers
{
    public class HierarchyObjectDbSetManager : NestedSetsDbSetManager<HierarchyObject>
    {
        public HierarchyObjectDbSetManager(DbSet<HierarchyObject> entities, IDbContainer dbContainer)
            : base(entities, dbContainer, "HierarchyObjects") { }
    }
}