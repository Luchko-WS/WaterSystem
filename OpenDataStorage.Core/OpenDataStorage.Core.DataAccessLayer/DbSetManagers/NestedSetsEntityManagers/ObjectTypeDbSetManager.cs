using OpenDataStorage.Core.DataAccessLayer.DbContext;
using OpenDataStorage.Core.DataAccessLayer.DbSetManagers.NestedSetsEntityManagers.Core;
using OpenDataStorage.Core.Entities.NestedSets;
using System.Data.Entity;
using System.Linq;

namespace OpenDataStorage.Core.DataAccessLayer.DbSetManagers.NestedSetsEntityManagers
{
    public class ObjectTypeDbSetManager : FSNestedSetsDbSetManager<ObjectType>
    {
        public ObjectTypeDbSetManager(DbSet<ObjectType> entities, IDbContainer dbContainer)
            : base(entities, dbContainer, "ObjectTypes") { }

        protected override IQueryable<ObjectType> IncludeAllDependencies(IQueryable<ObjectType> query)
        {
            return query;
        }
    }
}