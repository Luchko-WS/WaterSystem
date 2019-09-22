using OpenDataStorage.Common.Attributes;
using OpenDataStorage.Core.DataAccessLayer.DbSetManagers.BaseEntityDbSetManagers.Aliases;
using OpenDataStorage.Helpers;
using OpenDataStorageCore.Entities.Aliases;
using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace OpenDataStorage.API.Aliases
{
    [RoutePrefix("api/CharacteristicAliases")]
    [WebApiAuthorize(Roles = RolesHelper.DATA_MANAGEMENT_GROUP)]
    public class CharacteristicAliasesController : BaseAliasesController<CharacteristicAlias>
    {
        protected override IAliasDbSetManager<CharacteristicAlias> _DbSetManager => _dbContext.CharacteristicAliasDbSetManager;

        [AllowAnonymous]
        [Route("Get/{id}")]
        [HttpGet]
        public async Task<CharacteristicAlias> Get(Guid id)
        {
            return await base.GetInternal(id);
        }

        [AllowAnonymous]
        [Route("GetAllForCharacteristic/{id}")]
        [HttpGet]
        public async Task<dynamic> GetAliasesForCharacteristic(Guid id)
        {
            return await _DbSetManager.GetAllQueryWithAllDependencies()
                .Where(a => a.CharacteristicId == id)
                .OrderBy(a => a.Value)
                .ToListAsync();
        }

        [Route("Create/{characteristicId}")]
        [HttpPost]
        public async Task<HttpResponseMessage> Create([FromUri]Guid characteristicId, CharacteristicAlias vm)
        {
            var isCharacteristicExist = await _dbContext.CharacteristicContext.Entities.AnyAsync(c => c.Id == characteristicId);
            if (isCharacteristicExist)
            {
                var entity = new CharacteristicAlias
                {
                    CharacteristicId = characteristicId,
                    CreationDate = DateTime.Now.Date,
                    OwnerId = User.Identity.Name,
                    Value = vm.Value
                };
                return await base.CreateInternal(entity);
            }
            return Request.CreateErrorResponse(HttpStatusCode.NotFound, $"Characteristic {characteristicId} not found");
        }

        [Route("Update")]
        [HttpPut]
        public async Task<HttpResponseMessage> Update(CharacteristicAlias entity)
        {
            return await UpdateInternal(entity);
        }

        [Route("Delete/{id}")]
        [HttpDelete]
        public async Task<HttpResponseMessage> Delete(Guid id)
        {
            return await base.DeleteInternal(id);
        }
    }
}
