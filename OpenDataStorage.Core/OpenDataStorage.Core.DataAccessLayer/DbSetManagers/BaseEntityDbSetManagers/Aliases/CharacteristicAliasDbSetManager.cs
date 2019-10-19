using System.Data.Entity;
using System.Linq;
using OpenDataStorage.Common;
using OpenDataStorage.Core.DataAccessLayer.DbContext;
using OpenDataStorage.Core.Entities.Aliases;

namespace OpenDataStorage.Core.DataAccessLayer.DbSetManagers.BaseEntityDbSetManagers.Aliases
{
    public class CharacteristicAliasDbSetManager : BaseEntityDbSetManager<CharacteristicAlias>,
        IAliasDbSetManager<CharacteristicAlias>
    {
        public CharacteristicAliasDbSetManager(DbSet<CharacteristicAlias> entities, IDbContextBase dbContext)
            : base(entities, dbContext, "CharacteristicAliases") { }

        protected override IQueryable<CharacteristicAlias> IncludeAllDependencies(IQueryable<CharacteristicAlias> query)
        {
            return query.Include(e => e.Characteristic);
        }
    }
}
