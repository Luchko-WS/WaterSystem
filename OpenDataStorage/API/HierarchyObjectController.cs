using OpenDataStorage.Common;
using OpenDataStorage.ViewModels.HierarchyObjectViewModels;
using OpenDataStorageCore;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace OpenDataStorage.API
{
    [RoutePrefix("api/HierarchyObjects")]
    public class HierarchyObjectController : BaseApiController
    {
        [Route("GetTree")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<dynamic> GetTree([FromUri] HierarchyObjectFilterViewModel vm)
        {
            if (vm != null && !string.IsNullOrEmpty(vm.Name))
            {
                var ids = await _dbContext.HierarchyObjectContext.Entities
                    .Where(e => e.Name.ToLower().Contains(vm.Name.ToLower()))
                    .Select(e => e.Id).ToListAsync();

                var results = new List<HierarchyObject>();
                foreach (var id in ids)
                {
                    if (!results.Any(e => e.Id == id))
                    {
                        var branch = await _dbContext.HierarchyObjectContext.GetParentNodes(id, includeItself: true);
                        results = results.Union(branch).ToList();
                    }
                }
                return results.OrderBy(e => e.LeftKey).ToList();
            }
            return await _dbContext.HierarchyObjectContext.GetTree();
        }

        [Route("GetSubTree")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<HttpResponseMessage> GetSubTree([FromUri]HierarchyObjectViewModel vm)
        {
            try
            {
                var res = await _dbContext.HierarchyObjectContext.GetChildNodes(vm.Id);
                return Request.CreateResponse(HttpStatusCode.OK, res);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }
        }

        [Route("Get/{hierarchyObjectId}")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<HttpResponseMessage> Get([FromUri]Guid hierarchyObjectId)
        {
            try
            {
                var res = await _dbContext.HierarchyObjectContext.GetNode(hierarchyObjectId);
                return Request.CreateResponse(HttpStatusCode.OK, res);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }
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

            try
            {
                await _dbContext.HierarchyObjectContext.AddObject(entity, parentId);
                return Request.CreateResponse(HttpStatusCode.OK, entity);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        [Route("Update")]
        [HttpPut]
        public async Task<HttpResponseMessage> Update(HierarchyObjectViewModel vm)
        {
            try
            {
                vm.ObjectTypeId = vm.ObjectType?.Id;
                var entity = Mapper.CreateInstanceAndMapProperties<HierarchyObject>(vm);
                await _dbContext.HierarchyObjectContext.UpdateObject(entity);
                return Request.CreateResponse(HttpStatusCode.OK, entity);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        [Route("Delete/{id}")]
        [HttpDelete]
        public async Task<HttpResponseMessage> Delete(Guid id)
        {
            try
            {
                //redundant call
                var node = await _dbContext.HierarchyObjectContext.Entities.FirstOrDefaultAsync(e => e.Id == id);
                var parent = await _dbContext.HierarchyObjectContext.GetParentNode(id);
                await _dbContext.HierarchyObjectContext.RemoveObject(node);
                return Request.CreateResponse(HttpStatusCode.OK, parent);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }
    }
}
