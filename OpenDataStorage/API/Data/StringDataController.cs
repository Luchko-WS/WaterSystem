using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using OpenDataStorage.ViewModels.CharacteristicValueViewModel;
using OpenDataStorage.Helpers;
using OpenDataStorage.Common.Attributes;
using OpenDataStorageCore.Entities.NestedSets;
using OpenDataStorageCore.Entities.CharacteristicValues;

namespace OpenDataStorage.API.Data
{
    [RoutePrefix("api/Data")]
    [WebApiAuthorize(Roles = RolesHelper.DATA_MANAGEMENT_GROUP)]
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
                CharacteristicId = characteristicId,
                HierarchyObjectId = objectId,
                IsTimeIntervalValue = vm.IsTimeIntervalValue,
                CreationDate = vm.CreationDate,
                EndCreationDate = vm.IsTimeIntervalValue ? vm.EndCreationDate : null,
                Value = vm.Value,
                OwnerId = User.Identity.Name
            };

            return await CreateInner(objectId, characteristicId, entity);
        }

        [Route("String")]
        [HttpPut]
        public async Task<HttpResponseMessage> Update(StringValueViewModel entity)
        {
            var errorRes = await ValidateObjectAndCharacteristic(entity.HierarchyObjectId, entity.CharacteristicId, _supportedCharacteristicType);
            if (errorRes != null) return errorRes;
            return await UpdateInner(entity);
        }
    }
}