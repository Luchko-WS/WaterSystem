using OpenDataStorage.Common.DbContext.Managers.NestedSetsManagers.Core;
using OpenDataStorageCore.Entities.NestedSets;
using System.Data.Entity;

namespace OpenDataStorage.Common.DbContext.Managers.NestedSetsManagers
{
    public class ObjectTypeDbSetManager : FSNestedSetsDbSetManager<ObjectType>
    {
        public ObjectTypeDbSetManager(DbSet<ObjectType> entities, IDbContainer dbContainer)
            : base(entities, dbContainer, "ObjectTypes") { }
    }
}