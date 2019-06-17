﻿using OpenDataStorage.Common.Attributes;
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
        public CharacteristicAliasesController()
        {
            _dbSetManager = _dbContext.CharacteristicAliasDbSetManager;
        }
        
        [AllowAnonymous]
        [Route("Get/{id}")]
        [HttpDelete]
        public async Task<CharacteristicAlias> Get(Guid id)
        {
            return await base.GetInner(id);
        }

        [AllowAnonymous]
        [Route("GetAllForCharacteristic/{id}")]
        [HttpDelete]
        public async Task<dynamic> GetAliasesForCharacteristic(Guid id)
        {
            return await _dbSetManager.GetAllQuery().Where(a => a.CharacteristicId == id).ToListAsync();
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
                return await base.CreateInner(entity);
            }
            return Request.CreateErrorResponse(HttpStatusCode.NotFound, $"Characteristic {characteristicId} not found");
        }

        [Route("Update")]
        [HttpPut]
        public async Task<HttpResponseMessage> Update(CharacteristicAlias entity)
        {
            return await UpdateInner(entity);
        }

        [Route("Delete/{id}")]
        [HttpDelete]
        public async Task<HttpResponseMessage> Delete(Guid id)
        {
            return await base.DeleteInner(id);
        }
    }
}
