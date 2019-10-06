using OpenDataStorage.Core.DataAccessLayer.DbContext;
using OpenDataStorage.Core.DataAccessLayer.DbSetManagers.NestedSetsEntityManagers.Core;
using OpenDataStorage.Core.Entities.NestedSets;
using System.Data.Entity;
using System.Linq;

namespace OpenDataStorage.Core.DataAccessLayer.DbSetManagers.NestedSetsEntityManagers
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