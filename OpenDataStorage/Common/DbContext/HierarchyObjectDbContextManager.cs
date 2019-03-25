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
        }

        protected override Task PostAdd(HierarchyObject @object, NestedSetsFolder parentFolder)
        {
            throw new System.NotImplementedException();
        }

        protected override Task PostAddFolder(NestedSetsFolder folder, NestedSetsFolder parentFolder)
        {
            throw new System.NotImplementedException();
        }

        /*
         [DbFunction("BeeDbContext", "GetAppPublicStatistic")]
		public IQueryable<AppPublicStatistic> GetAppPublicStatistic()
		{
			return
				((IObjectContextAdapter)this).ObjectContext.CreateQuery<AppPublicStatistic>(
					$"[{GetType().Name}].{"[GetAppPublicStatistic]()"}");
		}
             */

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

        protected override Task PostRemoveFolder(NestedSetsFolder folder)
        {
            throw new System.NotImplementedException();
        }

        protected override Task PreAdd(HierarchyObject @object, NestedSetsFolder parentFolder)
        {
            throw new System.NotImplementedException();
        }

        protected override Task PreAddFolder(NestedSetsFolder folder, NestedSetsFolder parentFolder)
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

        protected override Task PreRemoveFolder(NestedSetsFolder folder)
        {
            throw new System.NotImplementedException();
        }
    }
}