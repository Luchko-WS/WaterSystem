using OpenDataStorageCore;
using System;
using System.Data.Entity;
using System.Threading.Tasks;

namespace OpenDataStorage.Common.DbContext
{
    public class HierarchyObjectDbContextManager : BaseNestedSetsDbContextManager<HierarchyObject>
    {
        public HierarchyObjectDbContextManager(DbSet<HierarchyObject> entities, Database database)
            : base(entities, database)
        {
            TableName = "HierarchyObject";
        }

        protected override async Task AddObjectInternal(HierarchyObject @object)
        {
            await ExecuteInsertSqlCommand(@object);
        }

        protected override async Task UpdateObjectInternal(HierarchyObject @object)
        {
            await ExecuteUpdateSqlCommand(@object);
        }

        protected override async Task MoveObjetInternal()
        {
            throw new NotImplementedException();
        }

        protected override async Task RemoveObjectInternal(HierarchyObject @object)
        {
            await ExecuteDeleteSqlCommand(@object);
        }

        protected override async Task AddFolderInternal(NestedSetsFileSystemEntity folder)
        {
            await ExecuteInsertSqlCommand(folder);
        }

        protected override async Task UpdateFolderInternal(NestedSetsFileSystemEntity folder)
        {
            await ExecuteUpdateSqlCommand(folder);
        }

        protected override async Task MoveFolderInternal()
        {
            throw new NotImplementedException();
        }

        protected override async Task RemoveFolderInternal(NestedSetsFileSystemEntity folder)
        {
            await ExecuteDeleteSqlCommand(folder);
        }
    }
}