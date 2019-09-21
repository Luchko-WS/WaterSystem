using OpenDataStorage.Common;
using OpenDataStorage.Common.Attributes;
using OpenDataStorage.Common.DbContext.Managers.NestedSetsManagers.Core;
using OpenDataStorage.Helpers;
using OpenDataStorage.ViewModels.HierarchyObjectViewModels;
using OpenDataStorageCore.Entities.NestedSets;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace OpenDataStorage.API.NestedSets
{
    [RoutePrefix("api/HierarchyObjects")]
    [WebApiAuthorize(Roles = RolesHelper.DATA_MANAGEMENT_GROUP)]
    public class HierarchyObjectController : BaseNestedSetsController<HierarchyObject>
    {
        protected override INestedSetsDbSetManager<HierarchyObject> _DbSetManager => _dbContext.HierarchyObjectContext;

        [Route("GetTree")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<dynamic> GetTree([FromUri] HierarchyObjectFilterViewModel vm)
        {
            if (vm != null && !string.IsNullOrEmpty(vm.Name))
            {
                var ids = await _dbContext.HierarchyObjectContext.Entities
                    .Include(e => e.HierarchyObjectAliases)
                    .Where(e => e.Name.ToLower().Contains(vm.Name.ToLower()) || 
                        e.HierarchyObjectAliases.Any(a => a.Value.ToLower().Contains(vm.Name.ToLower())))
                    .Select(e => e.Id).ToListAsync();

                var results = new List<HierarchyObject>();
                foreach (var id in ids)
                {
                    if (!results.Any(e => e.Id == id))
                    {
                        var branch = await _dbContext.HierarchyObjectContext.GetParentNodes(id, true, e => e.ObjectType);
                        results = results.Union(branch).ToList();
                    }
                }
                return results.OrderBy(e => e.LeftKey).ToList();
            }
            return await _dbContext.HierarchyObjectContext.GetTree(e => e.ObjectType);
        }

        [Route("GetSubTree")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<HttpResponseMessage> GetSubTree([FromUri]HierarchyObjectViewModel vm)
        {
            return await GetSubTreeInternal(vm.Id);
        }

        [Route("Get/{hierarchyObjectId}")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<HttpResponseMessage> Get([FromUri]Guid hierarchyObjectId)
        {
            return await GetInternal<HierarchyObjectViewModel>(hierarchyObjectId);
        }

        [Route("Create/{parentId}")]
        [HttpPost]
        public async Task<HttpResponseMessage> Create([FromUri]Guid parentId, HierarchyObjectViewModel vm)
        {
            vm.ObjectTypeId = vm.ObjectType?.Id;
            var entity = new HierarchyObject
            {
                Name = vm.Name,
                Description = vm.Description,
                OwnerId = User.Identity.Name,
                ObjectTypeId = vm.ObjectTypeId
            };

            return await CreateInternal(entity, parentId);
        }

        [Route("Update")]
        [HttpPut]
        public async Task<HttpResponseMessage> Update(HierarchyObjectViewModel vm)
        {
            vm.ObjectTypeId = vm.ObjectType?.Id;
            var entity = Mapper.CreateInstanceAndMapProperties<HierarchyObject>(vm);
            return await UpdateInternal(entity);
        }

        [Route("Move/{id}/{parentId}")]
        [HttpPost]
        public async Task<HttpResponseMessage> Move([FromUri]Guid id, Guid parentId)
        {
            return await MoveInternal(id, parentId);
        }

        [Route("Delete/{id}")]
        [HttpDelete]
        public async Task<HttpResponseMessage> Delete(Guid id)
        {
            return await DeleteInternal(id);
        }
    }
}
