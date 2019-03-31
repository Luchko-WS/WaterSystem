using OpenDataStorageCore;
using System;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace OpenDataStorage.Common.DbContext
{
    public class CharacteristicDbContextManager : BaseNestedSetsDbContextManager<Characteristic>
    {
        public CharacteristicDbContextManager(ApplicationDbContext dbContext, DbSet<Characteristic> entities)
            : base(dbContext, entities)
        {
            TableName = "Characteristics";
        }

        protected override async Task AddObjectInternal(Characteristic @object)
        {
            await ExecuteInsertSqlCommand(@object);
        }

        protected override Task MoveObjetInternal()
        {
            throw new NotImplementedException();
        }

        protected override Task RemoveObjectInternal(Characteristic @object)
        {
            throw new NotImplementedException();
        }

        protected override async Task AddFolderInternal(NestedSetsFileSystemEntity folder)
        {
            await ExecuteInsertSqlCommand(folder);
        }

        protected override Task MoveFolderInternal()
        {
            throw new NotImplementedException();
        }

        protected override Task RemoveFolderInternal(NestedSetsFileSystemEntity folder)
        {
            throw new NotImplementedException();
        }
    }
}