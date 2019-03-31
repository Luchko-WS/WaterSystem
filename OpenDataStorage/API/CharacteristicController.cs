using OpenDataStorage.Common;
using OpenDataStorage.ViewModels.CharacteristicViewModel;
using OpenDataStorageCore;
using System;
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
        [Route("GetCharacteristicTree")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<dynamic> GetCharacteristicTree()
        {
            //test
            var collection = _dbContext.CharacteristicObjectContext.Entities;
            return collection;
        }

        [Route("GetCharacteristicSubTree")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<dynamic> GetCharacteristicSubTree([FromUri]CharacteristicViewModel filter)
        {
            throw new NotImplementedException();
        }

        [Route("Characteristic/{characteristicId}")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<HttpResponseMessage> GetCharacteristic([FromUri]Guid characteristicId)
        {
            throw new NotImplementedException();
        }

        [Route("CreateCharacteristic")]
        [HttpPost]
        public async Task<HttpResponseMessage> CreateCharacteristic(CharacteristicViewModel vm)
        {
            //test
            var parentFolder = _dbContext.CharacteristicObjectContext.Entities.Where(e => e.Type == EntityType.Folder).FirstOrDefault();

            var characteristic = new Characteristic
            {
                Name = vm.Name,
                Description = vm.Description,
                Type = EntityType.File,
                OwnerId = User.Identity.Name
            };
            await _dbContext.CharacteristicObjectContext.AddObject(characteristic, parentFolder.Id);
            return Request.CreateResponse(HttpStatusCode.OK, characteristic);
        }

        [Route("Edit/{characteristicId}")]
        [HttpPut]
        public async Task<HttpResponseMessage> EditCharacteristic([FromUri]Guid characteristicId, CharacteristicViewModel vm)
        {
            var characteristic = Mapper.CreateInstanceAndMapProperties<Characteristic>(vm);
            await _dbContext.CharacteristicObjectContext.UpdateObject(characteristic);
            return Request.CreateResponse(HttpStatusCode.OK, characteristic);
        }

        [Route("Remove/{id}")]
        [HttpDelete]
        public async Task<HttpResponseMessage> RemoveCharacteristic(Guid id)
        {
            await _dbContext.CharacteristicObjectContext.RemoveObject(id);
            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}
