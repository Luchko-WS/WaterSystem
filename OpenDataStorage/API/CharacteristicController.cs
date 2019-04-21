using OpenDataStorage.Common;
using OpenDataStorage.ViewModels.CharacteristicViewModel;
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
    [RoutePrefix("api/Characteristics")]
    public class CharacteristicController : BaseApiController
    {
        [Route("GetTree")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<ICollection<Characteristic>> GetTree()
        {
            return await _dbContext.CharacteristicObjectContext.GetTree();
        }

        [Route("GetSubTree")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<HttpResponseMessage> GetSubTree([FromUri]CharacteristicViewModel vm)
        {
            try
            {
                var res = await _dbContext.CharacteristicObjectContext.GetChildNodes(vm.Id);
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
                var res = await _dbContext.CharacteristicObjectContext.GetNode(id);
                return Request.CreateResponse(HttpStatusCode.OK, res);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }
        }

        [Route("Create/{parentFolderId}")]
        [HttpPost]
        public async Task<HttpResponseMessage> Create([FromUri]Guid parentFolderId, CharacteristicViewModel vm)
        {
            var entity = new Characteristic
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
                    await _dbContext.CharacteristicObjectContext.AddObject(entity, parentFolderId);
                }
                else
                {
                    await _dbContext.CharacteristicObjectContext.AddFolder(entity, parentFolderId);
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
        public async Task<HttpResponseMessage> Update(CharacteristicViewModel vm)
        {
            try
            {
                var entity = Mapper.CreateInstanceAndMapProperties<Characteristic>(vm);
                if (entity.Type == EntityType.File)
                {
                    await _dbContext.CharacteristicObjectContext.UpdateObject(entity);
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
                var entity = await _dbContext.CharacteristicObjectContext.Entities.FirstOrDefaultAsync(e => e.Id == id);
                var parent = await _dbContext.CharacteristicObjectContext.GetParentNode(id);

                if (entity.Type == EntityType.File)
                {
                    await _dbContext.CharacteristicObjectContext.RemoveObject(entity);
                }
                else
                {
                    await _dbContext.CharacteristicObjectContext.RemoveFolder(entity);
                }
                return Request.CreateResponse(HttpStatusCode.OK, parent);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }
    }
}
