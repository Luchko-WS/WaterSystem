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
        public HierarchyObjectAliasDbSetManager(DbSet<HierarchyObjectAlias> entities, Func<Task> SaveChangesFunction)
            : base(entities)
        {
            _saveChangesFunction = SaveChangesFunction;
            TableName = "HierarchyObjectAliases";
        }

        protected override async Task SaveChanges()
        {
            await _saveChangesFunction?.Invoke();
        }

        protected override IQueryable<HierarchyObjectAlias> IncludeDependencies(IQueryable<HierarchyObjectAlias> query)
        {
            return query.Include(e => e.HierarchyObject);
        }
    }
}
