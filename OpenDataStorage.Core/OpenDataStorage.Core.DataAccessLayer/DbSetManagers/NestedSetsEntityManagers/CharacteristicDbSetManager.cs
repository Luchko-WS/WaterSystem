using OpenDataStorage.Core.DataAccessLayer.DbContext;
using OpenDataStorage.Core.DataAccessLayer.DbSetManagers.NestedSetsEntityManagers.Core;
using OpenDataStorage.Core.Entities.NestedSets;
using System.Data.Entity;
using System.Linq;

namespace OpenDataStorage.Core.DataAccessLayer.DbSetManagers.NestedSetsEntityManagers
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