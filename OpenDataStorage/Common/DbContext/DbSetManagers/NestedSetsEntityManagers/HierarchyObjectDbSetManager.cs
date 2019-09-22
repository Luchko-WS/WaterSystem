using OpenDataStorage.Common.DbContext.DbSetManagers.NestedSetsEntityManagers.Core;
using OpenDataStorageCore.Entities.NestedSets;
using System.Data.Entity;
using System.Linq;

namespace OpenDataStorage.Common.DbContext.DbSetManagers.NestedSetsEntityManagers
{
    public class HierarchyObjectDbSetManager : NestedSetsDbSetManager<HierarchyObject>
    {
        public HierarchyObjectDbSetManager(DbSet<HierarchyObject> entities, IDbContainer dbContainer)
            : base(entities, dbContainer, "HierarchyObjects") { }

        protected override IQueryable<HierarchyObject> IncludeAllDependencies(IQueryable<HierarchyObject> query)
        {
            return query
                .Include(e => e.HierarchyObjectAliases)
                .Include(e => e.ObjectType);
        }
    }
}