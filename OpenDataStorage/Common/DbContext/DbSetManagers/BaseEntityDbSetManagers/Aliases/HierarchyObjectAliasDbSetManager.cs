using System.Data.Entity;
using System.Linq;
using OpenDataStorageCore.Entities.Aliases;

namespace OpenDataStorage.Common.DbContext.DbSetManagers.BaseEntityDbSetManagers.Aliases
{
    public class HierarchyObjectAliasDbSetManager : BaseEntityDbSetManager<HierarchyObjectAlias>, IAliasDbSetManager<HierarchyObjectAlias>
    {
        public HierarchyObjectAliasDbSetManager(DbSet<HierarchyObjectAlias> entities, IApplicationDbContextBase dbContext)
            : base(entities, dbContext, "HierarchyObjectAliases") { }

        protected override IQueryable<HierarchyObjectAlias> IncludeAllDependencies(IQueryable<HierarchyObjectAlias> query)
        {
            return query.Include(e => e.HierarchyObject);
        }
    }
}
