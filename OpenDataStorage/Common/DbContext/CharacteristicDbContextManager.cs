using OpenDataStorageCore;
using System;
using System.Data.Entity;
using System.Threading.Tasks;

namespace OpenDataStorage.Common.DbContext
{
    public class CharacteristicDbContextManager : BaseNestedSetsDbContextManager<Characteristic>
    {
        public CharacteristicDbContextManager(DbSet<Characteristic> entities, Database database)
            : base(entities, database)
        {
            TableName = "Characteristics";
        }

        protected override async Task AddObjectInternal(Characteristic @object)
        {
            await ExecuteInsertSqlCommand(@object);
        }

        protected override async Task UpdateObjectInternal(Characteristic @object)
        {
            await ExecuteUpdateSqlCommand(@object);
        }

        protected override async Task MoveObjetInternal()
        {
            throw new NotImplementedException();
        }

        protected override async Task RemoveObjectInternal(Characteristic @object)
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