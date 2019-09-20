﻿using OpenDataStorage.Common.DbContext.DbSetManagers.Aliases;
using OpenDataStorageCore.Entities.Aliases;
using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace OpenDataStorage.API.Aliases
{
    public abstract class BaseAliasesController<T> : BaseApiController where T: BaseAlias
    {
        protected abstract IAliasDbSetManager<T> _DbSetManager { get; }

        protected async Task<T> GetInternal(Guid id)
        {
            return await _DbSetManager.GetEntityQuery(id).FirstOrDefaultAsync();
        }

        protected async Task<HttpResponseMessage> CreateInternal(T entity)
        {
            var errResponce = await CheckDuplicates(entity.Value);
            if (errResponce != null) return errResponce;

            try
            {
                await _DbSetManager.Create(entity);
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        protected async Task<HttpResponseMessage> UpdateInternal(T entity)
        {
            var errResponce = await CheckDuplicates(entity.Value);
            if (errResponce != null) return errResponce;

            try
            {
                await _DbSetManager.Update(entity);
                return Request.CreateResponse(HttpStatusCode.OK, entity);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        protected async Task<HttpResponseMessage> DeleteInternal(Guid id)
        {
            try
            {
                await _DbSetManager.Delete(id);
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        private async Task<HttpResponseMessage> CheckDuplicates(string value)
        {
            var alias = await _DbSetManager.GetAllQuery().Where(a => a.Value.Equals(value, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefaultAsync();
            if (alias != null) return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, $"Aliase is exist. Id = {alias.Id}");
            return null;
        }
    }
}
