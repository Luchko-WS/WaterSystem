using OpenDataStorage.Helpers;
using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using OpenDataStorage.Common.Attributes;
using OpenDataStorageCore.Entities.CharacteristicValues;
using OpenDataStorageCore.Entities.NestedSets;

namespace OpenDataStorage.API.Data
{
    [RoutePrefix("api/Data")]
    [WebApiAuthorize(Roles = RolesHelper.DATA_MANAGEMENT_GROUP)]
    public class DataController : BaseApiController
    {
        [Route("GetDataForObject/{id}")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<dynamic> GetDataForObject([FromUri]Guid id)
        {
            var data = await _dbContext.CharacteristicValueDbSetManager.GetAllForObjectQueryWithAllDependencies(id).ToListAsync();
            return data.Select(v => v.ConvertToViewModelIfExists());
        }

        [Route("Get/{id}")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<dynamic> Get([FromUri]Guid id)
        {
            var data = await _dbContext.CharacteristicValueDbSetManager.GetQueryWithAllDependencies(id).FirstOrDefaultAsync();
            return data.ConvertToViewModelIfExists();
        }

        [Route("Delete/{id}")]
        [HttpDelete]
        public async Task<HttpResponseMessage> Delete(Guid id)
        {
            try
            {
                await _dbContext.CharacteristicValueDbSetManager.Delete(id);
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        protected async Task<HttpResponseMessage> CreateInner(Guid objectId, Guid characteristicId, BaseCharacteristicValue value)
        {
            try
            {
                await _dbContext.CharacteristicValueDbSetManager.Create(value);
                return Request.CreateResponse(HttpStatusCode.OK, value);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        protected async Task<HttpResponseMessage> UpdateInner(BaseCharacteristicValue value)
        {
            try
            {
                await _dbContext.CharacteristicValueDbSetManager.Update(value);
                return Request.CreateResponse(HttpStatusCode.OK, value);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        protected async Task<HttpResponseMessage> ValidateObjectAndCharacteristic(Guid objectId, Guid characteristicId, CharacteristicType type)
        {
            var obj = await _dbContext.HierarchyObjectContext.Entities.FirstOrDefaultAsync(o => o.Id == objectId);
            if (obj == null) return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Object not found");

            var characteristic = await _dbContext.CharacteristicContext.Entities.FirstOrDefaultAsync(c => c.Id == characteristicId);
            if (characteristic == null) return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Characteristic not found");

            if (characteristic.CharacteristicType != type) return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Characteristic does not support this type");
            return null;
        }
    }
}