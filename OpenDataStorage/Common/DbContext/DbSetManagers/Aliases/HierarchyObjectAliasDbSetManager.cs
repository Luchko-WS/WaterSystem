using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using OpenDataStorageCore.Entities.Aliases;

namespace OpenDataStorage.Common.DbContext.DbSetManagers.Aliases
{
    public class HierarchyObjectAliasDbSetManager : BaseDbSetManager<HierarchyObjectAlias>, IHierarchyObjectAliasDbSetManager
    {
        protected readonly Func<Task> _saveChangesFunction;
        public HierarchyObjectAliasDbSetManager(DbSet<HierarchyObjectAlias> entities, IApplicationDbContextBase dbContext)
            : base(entities, dbContext)
        {
            TableName = "HierarchyObjectAliases";
        }

        protected override IQueryable<HierarchyObjectAlias> IncludeDependencies(IQueryable<HierarchyObjectAlias> query)
        {
            return query.Include(e => e.HierarchyObject);
        }
    }
}
