using System.Data.Entity;
using System.Linq;
using OpenDataStorageCore.Entities.Aliases;

namespace OpenDataStorage.Common.DbContext.DbSetManagers.BaseEntityDbSetManagers.Aliases
{
    public class CharacteristicAliasDbSetManager : BaseEntityDbSetManager<CharacteristicAlias>, IAliasDbSetManager<CharacteristicAlias>
    {
        public CharacteristicAliasDbSetManager(DbSet<CharacteristicAlias> entities, IApplicationDbContextBase dbContext)
            : base(entities, dbContext, "CharacteristicAliases") { }

        protected override IQueryable<CharacteristicAlias> IncludeAllDependencies(IQueryable<CharacteristicAlias> query)
        {
            return query.Include(e => e.Characteristic);
        }
    }
}
