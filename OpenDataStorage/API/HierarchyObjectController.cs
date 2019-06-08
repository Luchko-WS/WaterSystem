using OpenDataStorage.Common;
using OpenDataStorage.Common.Attributes;
using OpenDataStorage.Helpers;
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
    [WebApiAuthorize(Roles = RolesHelper.DATA_MANAGEMENT_GROUP)]
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
            try
            {
                var res = await _dbContext.HierarchyObjectContext.GetChildNodes(vm.Id, true, e => e.ObjectType);
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
                var res = await _dbContext.HierarchyObjectContext.GetNode(hierarchyObjectId, e => e.ObjectType);
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
            try
            {
                vm.ObjectTypeId = vm.ObjectType?.Id;
                var entity = new HierarchyObject
                {
                    Name = vm.Name,
                    Description = vm.Description,
                    OwnerId = User.Identity.Name,
                    ObjectTypeId = vm.ObjectTypeId
                };

                await _dbContext.HierarchyObjectContext.Add(entity, parentId);
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
                await _dbContext.HierarchyObjectContext.Update(entity);
                return Request.CreateResponse(HttpStatusCode.OK, entity);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        [Route("Move/{id}/{parentId}")]
        [HttpPost]
        public async Task<HttpResponseMessage> Move([FromUri]Guid id, Guid parentId)
        {
            try
            {
                var entity = _dbContext.HierarchyObjectContext.Entities.FirstOrDefault(e => e.Id == id);
                await _dbContext.HierarchyObjectContext.Move(id, parentId);
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
                var node = await _dbContext.HierarchyObjectContext.Entities.FirstOrDefaultAsync(e => e.Id == id);
                var parent = await _dbContext.HierarchyObjectContext.GetParentNode(id);

                await _dbContext.HierarchyObjectContext.Remove(node);
                return Request.CreateResponse(HttpStatusCode.OK, parent);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }
    }
}
