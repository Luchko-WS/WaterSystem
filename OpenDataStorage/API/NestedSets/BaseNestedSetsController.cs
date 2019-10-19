using OpenDataStorage.Common;
using OpenDataStorage.Core.DataAccessLayer.DbSetManagers.NestedSetsEntityManagers.Core;
using OpenDataStorage.ViewModels.CharacteristicViewModel;
using OpenDataStorage.Core.Entities.NestedSets;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace OpenDataStorage.API.NestedSets
{
    public abstract class BaseNestedSetsController<T> : BaseApiController where T: NestedSetsEntity
    {
        protected abstract INestedSetsDbSetManager<T> _DbSetManager { get; }

        protected async Task<ICollection<Characteristic>> GetTreeInternal(
            CharacteristicFilterViewModel vm, params Expression<Func<T, object>>[] includedPath)
        {
            throw new NotImplementedException();
        }

        protected async Task<HttpResponseMessage> GetSubTreeInternal(
            Guid id, params Expression<Func<T, object>>[] includedPath)
        {
            try
            {
                var res = await _DbSetManager.GetChildrenAsync(id, true, includedPath);
                return Request.CreateResponse(HttpStatusCode.OK, res);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }
        }

        protected async Task<HttpResponseMessage> GetInternal<TRes>(
            Guid id, params Expression<Func<T, object>>[] includedPath) where TRes : T
        {
            try
            {
                var entity = await _DbSetManager.GetQuery(id, includedPath).FirstOrDefaultAsync();
                var res = Mapper.CreateInstanceAndMapProperties<TRes>(entity);
                return Request.CreateResponse(HttpStatusCode.OK, res);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }
        }

        protected async Task<HttpResponseMessage> CreateInternal(T entity, Guid parentId)
        {
            try
            {
                await _DbSetManager.CreateAsync(entity, parentId);
                return Request.CreateResponse(HttpStatusCode.OK, entity);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        protected async Task<HttpResponseMessage> UpdateInternal(T entity)
        {
            try
            {
                await _DbSetManager.UpdateAsync(entity);
                return Request.CreateResponse(HttpStatusCode.OK, entity);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        protected async Task<HttpResponseMessage> MoveInternal(Guid id, Guid parentId)
        {
            try
            {
                var entity = _DbSetManager.Entities.FirstOrDefault(e => e.Id == id); //change the logic!
                await _DbSetManager.MoveAsync(id, parentId);
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
                var parent = await _DbSetManager.GetParentAsync(id);
                await _DbSetManager.DeleteAsync(id);
                return Request.CreateResponse(HttpStatusCode.OK, parent);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }
    }
}
