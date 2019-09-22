﻿using OpenDataStorage.Common.DbContext.DbSetManagers.NestedSetsEntityManagers.Core;
using OpenDataStorageCore.Entities.NestedSets;
using System.Data.Entity;
using System.Linq;

namespace OpenDataStorage.Common.DbContext.DbSetManagers.NestedSetsEntityManagers
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