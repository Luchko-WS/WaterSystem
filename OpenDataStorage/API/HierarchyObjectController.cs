using OpenDataStorage.Common;
using OpenDataStorage.ViewModels.HierarchyObjectViewModels;
using OpenDataStorageCore;
using System;
using System.Data.Entity;
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
        public async Task<dynamic> GetTree()
        {
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

        [Route("Create/{parentFolderId}")]
        [HttpPost]
        public async Task<HttpResponseMessage> Create([FromUri]Guid parentFolderId, HierarchyObjectViewModel vm)
        {
            var entity = new HierarchyObject
            {
                Name = vm.Name,
                Description = vm.Description,
                Type = vm.Type,
                OwnerId = User.Identity.Name
            };

            try
            {
                if (entity.Type == EntityType.File)
                {
                    await _dbContext.HierarchyObjectContext.AddObject(entity, parentFolderId);
                }
                else
                {
                    await _dbContext.HierarchyObjectContext.AddFolder(entity, parentFolderId);
                }
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
                var entity = Mapper.CreateInstanceAndMapProperties<HierarchyObject>(vm);
                if (entity.Type == EntityType.File)
                {
                    await _dbContext.HierarchyObjectContext.UpdateObject(entity);
                }
                else
                {
                    await _dbContext.HierarchyObjectContext.UpdatFolder(entity);
                }
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
                if (node.Type == EntityType.File)
                {
                    await _dbContext.HierarchyObjectContext.RemoveObject(id);
                }
                else
                {
                    await _dbContext.HierarchyObjectContext.RemoveFolder(id);
                }
                return Request.CreateResponse(HttpStatusCode.OK, id);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }
    }
}
