using OpenDataStorage.Common;
using OpenDataStorage.Common.Attributes;
using OpenDataStorage.Common.DbContext.Managers.NestedSetsManagers.Core;
using OpenDataStorage.Helpers;
using OpenDataStorage.ViewModels.ObjectTypeViewModels;
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
    [RoutePrefix("api/ObjectType")]
    [WebApiAuthorize(Roles = RolesHelper.DATA_MANAGEMENT_GROUP)]
    public class ObjectTypeController : BaseNestedSetsController<ObjectType>
    {
        protected override INestedSetsDbSetManager<ObjectType> _DbSetManager => _dbContext.ObjectTypeContext;

        [Route("GetTree")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<ICollection<ObjectType>> GetTree([FromUri]ObjectTypeFilterViewModel vm)
        {
            if (vm != null && !string.IsNullOrEmpty(vm.Name))
            {
                var ids = await _dbContext.ObjectTypeContext.Entities
                    .Where(e => e.Name.ToLower().Contains(vm.Name.ToLower()))
                    .Select(e => e.Id).ToListAsync();

                var results = new List<ObjectType>();
                foreach(var id in ids)
                {
                    if (!results.Any(e => e.Id == id))
                    {
                        var branch = await _dbContext.ObjectTypeContext.GetParentNodes(id, includeItself: true);
                        results = results.Union(branch).ToList();
                    }
                }
                return results.OrderBy(e => e.LeftKey).ToList();
            }
            return await _dbContext.ObjectTypeContext.GetTree();
        }

        [Route("GetSubTree")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<HttpResponseMessage> GetSubTree([FromUri]ObjectTypeViewModel vm)
        {
            return await GetSubTreeInternal(vm.Id);
        }

        [Route("Get/{id}")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<HttpResponseMessage> Get([FromUri]Guid id)
        {
            return await GetInternal<ObjectType>(id);
        }

        [Route("Create/{parentFolderId}")]
        [HttpPost]
        public async Task<HttpResponseMessage> Create([FromUri]Guid parentFolderId, ObjectTypeViewModel vm)
        {
            var entity = new ObjectType
            {
                Name = vm.Name,
                Description = vm.Description,
                EntityType = vm.EntityType,
                OwnerId = User.Identity.Name
            };

            return await CreateInternal(entity, parentFolderId);
        }

        [Route("Update")]
        [HttpPut]
        public async Task<HttpResponseMessage> Update(ObjectTypeViewModel vm)
        {
            var entity = Mapper.CreateInstanceAndMapProperties<ObjectType>(vm);
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
