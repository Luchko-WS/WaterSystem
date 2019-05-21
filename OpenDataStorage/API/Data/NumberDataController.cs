using OpenDataStorage.Common;
using OpenDataStorageCore;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using OpenDataStorage.ViewModels.CharacteristicValueViewModel;

namespace OpenDataStorage.API.Data
{
    [RoutePrefix("api/Data")]
    public class NumberDataController: DataController
    {
        private readonly CharacteristicType _supportedCharacteristicType = CharacteristicType.Number;

        [Route("Number/{objectId}/{characteristicId}")]
        [HttpPost]
        public async Task<HttpResponseMessage> Create([FromUri]Guid objectId, Guid characteristicId, NumberValueViewModel vm)
        {
            var errorRes = await ValidateObjectAndCharacteristic(objectId, characteristicId, _supportedCharacteristicType);
            if (errorRes != null) return errorRes;

            var entity = new NumberCharacteristicValue
            {
                CharacterisitcId = characteristicId,
                HierarchyObjectId = objectId,
                CreationDate = vm.CreationDate ?? DateTime.Now,
                Value = vm.Value,
                OwnerId = User.Identity.Name
            };

            return await CreateInner(objectId, characteristicId, entity);
        }

        [Route("Number")]
        [HttpPut]
        public async Task<HttpResponseMessage> Update(NumberValueViewModel vm)
        {
            var entity = Mapper.CreateInstanceAndMapProperties<NumberCharacteristicValue>(vm);
            var errorRes = await ValidateObjectAndCharacteristic(entity.HierarchyObjectId, entity.CharacterisitcId, _supportedCharacteristicType);
            if (errorRes != null) return errorRes;
            return await UpdateInner(entity);
        }
    }
}