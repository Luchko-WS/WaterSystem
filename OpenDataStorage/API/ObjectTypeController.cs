using OpenDataStorage.Common;
using OpenDataStorage.ViewModels.HierarchyObjectTypeViewModels;
using OpenDataStorageCore;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace OpenDataStorage.API
{
    [RoutePrefix("api/ObjectTypeController")]
    public class ObjectTypeController : BaseApiController
    {
        [Route("GetTree")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<ICollection<HierarchyObjectType>> GetTree()
        {
            return await _dbContext.HierarchyObjectTypeContext.GetTree();
        }

        [Route("GetSubTree")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<HttpResponseMessage> GetSubTree([FromUri]HierarchyObjectTypeViewModel vm)
        {
            try
            {
                var res = await _dbContext.HierarchyObjectTypeContext.GetChildNodes(vm.Id);
                return Request.CreateResponse(HttpStatusCode.OK, res);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }
        }

        [Route("Get/{id}")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<HttpResponseMessage> Get([FromUri]Guid id)
        {
            try
            {
                var res = await _dbContext.HierarchyObjectTypeContext.GetNode(id);
                return Request.CreateResponse(HttpStatusCode.OK, res);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }
        }

        [Route("Create/{parentFolderId}")]
        [HttpPost]
        public async Task<HttpResponseMessage> Create([FromUri]Guid parentFolderId, HierarchyObjectTypeViewModel vm)
        {
            var entity = new HierarchyObjectType
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
                    await _dbContext.HierarchyObjectTypeContext.AddObject(entity, parentFolderId);
                }
                else
                {
                    await _dbContext.HierarchyObjectTypeContext.AddFolder(entity, parentFolderId);
                }
                return Request.CreateResponse(HttpStatusCode.OK, entity);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        [Route("Edit")]
        [HttpPut]
        public async Task<HttpResponseMessage> Edit(HierarchyObjectTypeViewModel vm)
        {
            try
            {
                var entity = Mapper.CreateInstanceAndMapProperties<HierarchyObjectType>(vm);
                if (entity.Type == EntityType.File)
                {
                    await _dbContext.HierarchyObjectTypeContext.UpdateObject(entity);
                }
                else
                {
                    await _dbContext.CharacteristicObjectContext.UpdatFolder(entity);
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
                var entity = await _dbContext.HierarchyObjectTypeContext.Entities.FirstOrDefaultAsync(e => e.Id == id);
                if (entity.Type == EntityType.File)
                {
                    await _dbContext.HierarchyObjectTypeContext.RemoveObject(id);
                }
                else
                {
                    await _dbContext.HierarchyObjectTypeContext.RemoveFolder(id);
                }
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }
    }
}
