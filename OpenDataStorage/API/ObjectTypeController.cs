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
    [RoutePrefix("api/ObjectType")]
    public class ObjectTypeController : BaseApiController
    {
        [Route("GetTree")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<ICollection<ObjectType>> GetTree()
        {
            return await _dbContext.ObjectTypeContext.GetTree();
        }

        [Route("GetSubTree")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<HttpResponseMessage> GetSubTree([FromUri]ObjectTypeViewModel vm)
        {
            try
            {
                var res = await _dbContext.ObjectTypeContext.GetChildNodes(vm.Id);
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
                var res = await _dbContext.ObjectTypeContext.GetNode(id);
                return Request.CreateResponse(HttpStatusCode.OK, res);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }
        }

        [Route("Create/{parentFolderId}")]
        [HttpPost]
        public async Task<HttpResponseMessage> Create([FromUri]Guid parentFolderId, ObjectTypeViewModel vm)
        {
            var entity = new ObjectType
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
                    await _dbContext.ObjectTypeContext.AddObject(entity, parentFolderId);
                }
                else
                {
                    await _dbContext.ObjectTypeContext.AddFolder(entity, parentFolderId);
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
        public async Task<HttpResponseMessage> Update(ObjectTypeViewModel vm)
        {
            try
            {
                var entity = Mapper.CreateInstanceAndMapProperties<ObjectType>(vm);
                if (entity.Type == EntityType.File)
                {
                    await _dbContext.ObjectTypeContext.UpdateObject(entity);
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
                var entity = await _dbContext.ObjectTypeContext.Entities.FirstOrDefaultAsync(e => e.Id == id);
                if (entity.Type == EntityType.File)
                {
                    await _dbContext.ObjectTypeContext.RemoveObject(id);
                }
                else
                {
                    await _dbContext.ObjectTypeContext.RemoveFolder(id);
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
