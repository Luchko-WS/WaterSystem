using OpenDataStorage.Common;
using OpenDataStorage.ViewModels.CharacteristicViewModel;
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
    [RoutePrefix("api/Characteristics")]
    public class CharacteristicController : BaseApiController
    {
        [Route("GetTree")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<ICollection<Characteristic>> GetTree([FromUri]CharacteristicFilterViewModel vm)
        {
            if (vm != null && !string.IsNullOrEmpty(vm.Name))
            {
                var ids = await _dbContext.CharacteristicObjectContext.Entities
                    .Where(e => e.Name.ToLower().Contains(vm.Name.ToLower()))
                    .Select(e => e.Id).ToListAsync();

                var results = new List<Characteristic>();
                foreach (var id in ids)
                {
                    if (!results.Any(e => e.Id == id))
                    {
                        var branch = await _dbContext.CharacteristicObjectContext.GetParentNodes(id, includeItself: true);
                        results = results.Union(branch).ToList();
                    }
                }
                return results.OrderBy(e => e.LeftKey).ToList();
            }
            return await _dbContext.CharacteristicObjectContext.GetTree();
        }

        [Route("GetSubTree")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<HttpResponseMessage> GetSubTree([FromUri]CharacteristicViewModel vm)
        {
            try
            {
                var res = await _dbContext.CharacteristicObjectContext.GetChildNodes(vm.Id, true);
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
            try
            {
                var entity = new Characteristic
                {
                    Name = vm.Name,
                    Description = vm.Description,
                    EntityType = vm.EntityType,
                    CharacteristicType = vm.EntityType == EntityType.Folder ? CharacteristicType.None : vm.CharacteristicType,
                    OwnerId = User.Identity.Name
                };

                await _dbContext.CharacteristicObjectContext.Add(entity, parentFolderId);
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
                await _dbContext.CharacteristicObjectContext.Update(entity);
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
                var entity = _dbContext.CharacteristicObjectContext.Entities.FirstOrDefault(e => e.Id == id);
                await _dbContext.CharacteristicObjectContext.Move(id, parentId);
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
                var entity = await _dbContext.CharacteristicObjectContext.Entities.FirstOrDefaultAsync(e => e.Id == id);
                var parent = await _dbContext.CharacteristicObjectContext.GetParentNode(id);

                await _dbContext.CharacteristicObjectContext.Remove(entity);
                return Request.CreateResponse(HttpStatusCode.OK, parent);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }
    }
}
