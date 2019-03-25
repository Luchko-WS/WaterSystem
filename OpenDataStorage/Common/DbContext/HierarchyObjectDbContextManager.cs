using OpenDataStorageCore;
using System.Data.Entity;
using System.Threading.Tasks;

namespace OpenDataStorage.Common.DbContext
{
    public class HierarchyObjectDbContextManager : BaseNestedSetsDbContextManager<HierarchyObject>
    {
        private ApplicationDbContext _dbContext;

        public HierarchyObjectDbContextManager(ApplicationDbContext dbContext)
            : base(dbContext.HierarchyObjects)
        {
            _dbContext = dbContext;
            TableName = "HierarchyObject";
        }

        protected override Task PostAdd(HierarchyObject @object, NestedSetsFileSystemEntity parentFolder)
        {
            throw new System.NotImplementedException();
        }

        protected override Task PostAddFolder(NestedSetsFileSystemEntity folder, NestedSetsFileSystemEntity parentFolder)
        {
            throw new System.NotImplementedException();
        }

        protected override Task PostMove()
        {
            throw new System.NotImplementedException();
        }

        protected override Task PostMoveFolder()
        {
            throw new System.NotImplementedException();
        }

        protected override Task PostRemove(HierarchyObject @object)
        {
            throw new System.NotImplementedException();
        }

        protected override Task PostRemoveFolder(NestedSetsFileSystemEntity folder)
        {
            throw new System.NotImplementedException();
        }

        protected override Task PreAdd(HierarchyObject @object, NestedSetsFileSystemEntity parentFolder)
        {
            throw new System.NotImplementedException();
        }

        protected override Task PreAddFolder(NestedSetsFileSystemEntity folder, NestedSetsFileSystemEntity parentFolder)
        {
            throw new System.NotImplementedException();
        }

        protected override Task PreMove()
        {
            throw new System.NotImplementedException();
        }

        protected override Task PreMoveFolder()
        {
            throw new System.NotImplementedException();
        }

        protected override Task PreRemove(HierarchyObject @object)
        {
            throw new System.NotImplementedException();
        }

        protected override Task PreRemoveFolder(NestedSetsFileSystemEntity folder)
        {
            throw new System.NotImplementedException();
        }
    }
}