﻿using OpenDataStorage.Common;
using OpenDataStorage.ViewModels.CharacteristicViewModel;
using OpenDataStorageCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace OpenDataStorage.API
{
    [RoutePrefix("api/Characteristic")]
    public class CharacteristicController : BaseApiController
    {
        [Route("GetCharacteristicTree")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<dynamic> GetCharacteristicTree()
        {
            var collection = _dbContext.CharacteristicObjectContext.Entities;
            return collection;
            /*var query = _dbContext.Dictionaries.Where(d => d.IsPublic);
            query = PrepareQueryByFilter(query, filter);
            var dictionaries = await query.OrderByDescending(d => d.CreationDate).ToListAsync();
            return Request.CreateResponse(HttpStatusCode.OK, dictionaries);*/
        }

        [Route("GetCharacteristicSubTree")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<dynamic> GetCharacteristicSubTree([FromUri]CharacteristicViewModel filter)
        {
            throw new NotImplementedException();
            /*var query = _dbContext.Dictionaries.Where(d => d.OwnerId == User.Identity.Name);
            query = PrepareQueryByFilter(query, filter);
            var dictionaries = await query.OrderByDescending(d => d.CreationDate).ToListAsync();
            return Request.CreateResponse(HttpStatusCode.OK, dictionaries);*/
        }

        [Route("Characteristic/{characteristicId}")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<HttpResponseMessage> GetCharacteristic([FromUri]Guid characteristicId)
        {
            throw new NotImplementedException();
            /*var dictionary = await _dbContext.Dictionaries
                .Include(d => d.PhrasesPairs)
                .Include(d => d.PhrasesPairs.Select(p => p.FirstPhrase))
                .Include(d => d.PhrasesPairs.Select(p => p.SecondPhrase))
                .FirstOrDefaultAsync(d => d.Id == dictionaryId);

            if (dictionary == null) return Request.CreateResponse(HttpStatusCode.NotFound);
            if (!dictionary.IsPublic && dictionary.OwnerId != User.Identity.Name) return Request.CreateResponse(HttpStatusCode.Forbidden);

            return Request.CreateResponse(HttpStatusCode.OK, res);*/
        }

        [Route("CreateCharacteristic")]
        [HttpPost]
        public async Task<HttpResponseMessage> CreateCharacteristic(CharacteristicViewModel vm)
        {
            var parentFolder = _dbContext.CharacteristicObjectContext.Entities.Where(e => e.Type == EntityType.Folder).FirstOrDefault();

            var characteristic = new Characteristic
            {
                Name = vm.Name,
                Description = vm.Description,
                Type = EntityType.File,
                OwnerId = User.Identity.Name
            };
            await _dbContext.CharacteristicObjectContext.AddObject(characteristic, parentFolder);
            return Request.CreateResponse(HttpStatusCode.OK, characteristic);
        }

        [Route("Edit/{characteristicId}")]
        [HttpPut]
        public async Task<HttpResponseMessage> EditCharacteristic([FromUri]Guid characteristicId, CharacteristicViewModel vm)
        {
            throw new NotImplementedException();
            /*var dictionary = await _dbContext.Dictionaries.FirstOrDefaultAsync(d => d.Id == dictionaryId);
            if (dictionary != null)
            {
                dictionary.Name = vm.Name;
                dictionary.Description = vm.Description;
                dictionary.IsPublic = vm.IsPublic;
                dictionary.LastChangeDate = DateTime.Now;

                await _dbContext.SaveDbChangesAsync();
                return Request.CreateResponse(HttpStatusCode.OK, dictionary);
            }
            return Request.CreateResponse(HttpStatusCode.NotFound);*/
        }

        [Route("Remove/{id}")]
        [HttpDelete]
        public async Task<HttpResponseMessage> RemoveCharacteristic(Guid id)
        {
            throw new NotImplementedException();
            /*var dictionaryToRemove = await _dbContext.Dictionaries.FirstOrDefaultAsync(d => d.Id == id);
            if (dictionaryToRemove != null)
            {
                var res = await _dbContext.RemoveDictionary(dictionaryToRemove);
                await _dbContext.SaveDbChangesAsync();
                return Request.CreateResponse(HttpStatusCode.OK, res);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }*/
        }
    }
}
