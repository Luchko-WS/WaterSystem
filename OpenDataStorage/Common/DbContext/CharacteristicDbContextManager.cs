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

        protected override Task AddObjectInternal(Characteristic @object)
        {
            throw new NotImplementedException();
            //insert into employee (ID, name, salary, start_date, city, region)
            //values(6, 'James', 70060, '09/06/99', 'Toronto', 'N')
        }

        protected override Task MoveObjetInternal()
        {
            throw new NotImplementedException();
        }

        protected override Task RemoveObjectInternal(Characteristic @object)
        {
            throw new NotImplementedException();
        }

        protected override Task AddFolderInternal(NestedSetsFileSystemEntity folder)
        {
            throw new NotImplementedException();
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