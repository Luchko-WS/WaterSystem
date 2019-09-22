using OpenDataStorage.Common.DbContext.DbSetManagers.NestedSetsEntityManagers.Core;
using OpenDataStorageCore.Entities.NestedSets;
using System.Data.Entity;
using System.Linq;

namespace OpenDataStorage.Common.DbContext.DbSetManagers.NestedSetsEntityManagers
{
    public class CharacteristicDbSetManager : ExtendedFSNestedSetsDbSetManaget<Characteristic>
    {
        public CharacteristicDbSetManager(DbSet<Characteristic> entities, IDbContainer dbContainer)
            : base(entities, dbContainer, "Characteristics") { }

        protected override IQueryable<Characteristic> IncludeAllDependencies(IQueryable<Characteristic> query)
        {
            return query.Include(e => e.CharacteristicAliases);
        }
    }
}