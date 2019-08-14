using OpenDataStorage.Common.DbContext.DbSetManagers.Aliases;
using OpenDataStorageCore.Entities.Aliases;
using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace OpenDataStorage.API.Aliases
{
    public class BaseAliasesController<T> : BaseApiController where T: BaseAlias
    {
        protected IAliasDbSetManager<T> _dbSetManager;

        protected async Task<T> GetInner(Guid id)
        {
            return await _dbSetManager.GetEntityQuery(id).FirstOrDefaultAsync();
        }

        protected async Task<HttpResponseMessage> CreateInner(T entity)
        {
            var errResponce = await CheckDuplicates(entity.Value);
            if (errResponce != null) return errResponce;

            try
            {
                await _dbSetManager.Create(entity);
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        protected async Task<HttpResponseMessage> UpdateInner(T entity)
        {
            var errResponce = await CheckDuplicates(entity.Value);
            if (errResponce != null) return errResponce;

            try
            {
                await _dbSetManager.Update(entity);
                return Request.CreateResponse(HttpStatusCode.OK, entity);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        protected async Task<HttpResponseMessage> DeleteInner(Guid id)
        {
            try
            {
                await _dbSetManager.Delete(id);
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        private async Task<HttpResponseMessage> CheckDuplicates(string value)
        {
            var alias = await _dbSetManager.GetAllQuery().Where(a => a.Value.Equals(value, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefaultAsync();
            if (alias != null) return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, $"Aliase is exist. Id = {alias.Id}");
            return null;
        }
    }
}
