using OpenDataStorage.Common;
using OpenDataStorageCore;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using OpenDataStorage.ViewModels.CharacteristicValueViewModel;

namespace OpenDataStorage.API.Data
{
    public class StringDataController : DataController
    {
        private readonly CharacteristicType _supportedCharacteristicType = CharacteristicType.String;

        [Route("String/{objectId}/{characteristicId}")]
        [HttpPost]
        public async Task<HttpResponseMessage> Create([FromUri]Guid objectId, Guid characteristicId, StringValueViewModel vm)
        {
            var errorRes = await ValidateObjectAndCharacteristic(objectId, characteristicId, _supportedCharacteristicType);
            if (errorRes != null) return errorRes;

            var entity = new StringCharacteristicValue
            {
                CharacterisitcId = characteristicId,
                HierarchyObjectId = objectId,
                CreationDate = vm.CreationDate ?? DateTime.Now,
                Value = vm.Value,
                OwnerId = User.Identity.Name
            };

            return await CreateInner(objectId, characteristicId, entity);
        }

        [Route("String")]
        [HttpPut]
        public async Task<HttpResponseMessage> Update(StringValueViewModel vm)
        {
            var entity = Mapper.CreateInstanceAndMapProperties<StringCharacteristicValue>(vm);
            var errorRes = await ValidateObjectAndCharacteristic(entity.HierarchyObjectId, entity.CharacterisitcId, _supportedCharacteristicType);
            if (errorRes != null) return errorRes;
            return await UpdateInner(entity);
        }
    }
}