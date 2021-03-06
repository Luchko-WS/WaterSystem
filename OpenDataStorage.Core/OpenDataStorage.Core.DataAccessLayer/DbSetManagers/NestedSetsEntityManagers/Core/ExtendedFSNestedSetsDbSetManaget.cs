﻿using OpenDataStorage.Core.DataAccessLayer.DbContext;
using OpenDataStorage.Core.Entities.NestedSets;
using System;
using System.Data.Entity;
using System.Threading.Tasks;

namespace OpenDataStorage.Core.DataAccessLayer.DbSetManagers.NestedSetsEntityManagers.Core
{
    public abstract class ExtendedFSNestedSetsDbSetManaget<T> : FSNestedSetsDbSetManager<T> where T : NestedSetsFSEntity
    {
        public ExtendedFSNestedSetsDbSetManaget(DbSet<T> entities, IDbContainer dbContainer, string tableName)
            : base(entities, dbContainer, tableName) { }

        public override async Task<Guid> CreateAsync(T entity, Guid parentId)
        {
            var parentNode = await CheckAndGetEntityByIdAsync(parentId);
            if (entity.EntityType == EntityType.Folder && parentNode.EntityType == EntityType.File)
            {
                throw new ArgumentException(string.Format("Cannot create child folder in entity {0} in {1} table.",
                    parentId, TableName));
            }
            return await ExecuteCreateAsync(entity, parentNode);
        }

        public override async Task MoveAsync(Guid id, Guid parentId)
        {
            var entity = await CheckAndGetEntityByIdAsync(id);
            var parentNode = await CheckAndGetEntityByIdAsync(parentId);
            if (entity.EntityType == EntityType.Folder && parentNode.EntityType == EntityType.File)
            {
                throw new ArgumentException(string.Format("Cannot create child folder in entity {0} in {1} table.",
                    parentId, TableName));
            }
            await ExecuteMoveAsync(entity, parentNode);
        }
    }
}