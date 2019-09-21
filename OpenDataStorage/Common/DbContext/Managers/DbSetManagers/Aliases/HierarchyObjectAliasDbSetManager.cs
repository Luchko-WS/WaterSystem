using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using OpenDataStorageCore.Entities.Aliases;

namespace OpenDataStorage.Common.DbContext.Managers.DbSetManagers.Aliases
{
    public class HierarchyObjectAliasDbSetManager : BaseDbSetManager<HierarchyObjectAlias>, IAliasDbSetManager<HierarchyObjectAlias>
    {
        protected readonly Func<Task> _saveChangesFunction;
        public HierarchyObjectAliasDbSetManager(DbSet<HierarchyObjectAlias> entities, IApplicationDbContextBase dbContext)
            : base(entities, dbContext, "HierarchyObjectAliases") { }

        protected override IQueryable<HierarchyObjectAlias> IncludeDependencies(IQueryable<HierarchyObjectAlias> query)
        {
            return query.Include(e => e.HierarchyObject);
        }
    }
}
