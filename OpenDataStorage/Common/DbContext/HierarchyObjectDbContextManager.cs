using OpenDataStorageCore;
using System.Data.Entity;
using System.Threading.Tasks;

namespace OpenDataStorage.Common.DbContext
{
    public class HierarchyObjectDbContextManager : BaseNestedSetsDbContextManager<HierarchyObject>
    {
        public HierarchyObjectDbContextManager(ApplicationDbContext dbContext, DbSet<HierarchyObject> entities)
            : base(dbContext, entities)
        {
            TableName = "HierarchyObject";
        }

        protected override Task AddObjectInternal(HierarchyObject @object)
        {
            throw new System.NotImplementedException();
        }

        protected override Task MoveObjetInternal()
        {
            throw new System.NotImplementedException();
        }

        protected override Task RemoveObjectInternal(HierarchyObject @object)
        {
            throw new System.NotImplementedException();
        }

        protected override Task AddFolderInternal(NestedSetsFileSystemEntity folder)
        {
            throw new System.NotImplementedException();
        }

        protected override Task MoveFolderInternal()
        {
            throw new System.NotImplementedException();
        }

        protected override Task RemoveFolderInternal(NestedSetsFileSystemEntity folder)
        {
            throw new System.NotImplementedException();
        }
    }
}