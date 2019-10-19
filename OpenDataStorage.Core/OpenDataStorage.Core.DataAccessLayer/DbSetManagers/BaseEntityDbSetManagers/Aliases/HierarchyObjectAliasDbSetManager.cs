using System.Data.Entity;
using System.Linq;
using OpenDataStorage.Common;
using OpenDataStorage.Core.DataAccessLayer.DbContext;
using OpenDataStorage.Core.Entities.Aliases;

namespace OpenDataStorage.Core.DataAccessLayer.DbSetManagers.BaseEntityDbSetManagers.Aliases
{
    public class HierarchyObjectAliasDbSetManager : BaseEntityDbSetManager<HierarchyObjectAlias>,
        IAliasDbSetManager<HierarchyObjectAlias>
    {
        public HierarchyObjectAliasDbSetManager(DbSet<HierarchyObjectAlias> entities, IDbContextBase dbContext)
            : base(entities, dbContext, "HierarchyObjectAliases") { }

        protected override IQueryable<HierarchyObjectAlias> IncludeAllDependencies(IQueryable<HierarchyObjectAlias> query)
        {
            return query.Include(e => e.HierarchyObject);
        }
    }
}
