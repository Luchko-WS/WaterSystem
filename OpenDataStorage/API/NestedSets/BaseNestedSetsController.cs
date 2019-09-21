using OpenDataStorage.Common;
using OpenDataStorage.Common.DbContext.Managers.NestedSetsManagers.Core;
using OpenDataStorage.ViewModels.CharacteristicViewModel;
using OpenDataStorageCore.Entities.NestedSets;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace OpenDataStorage.API.NestedSets
{
    public abstract class BaseNestedSetsController<T> : BaseApiController where T: NestedSetsEntity
    {
        protected abstract INestedSetsDbSetManager<T> _DbSetManager { get; }

        protected async Task<ICollection<Characteristic>> GetTreeInternal([FromUri]CharacteristicFilterViewModel vm)
        {
            throw new NotImplementedException();
        }

        protected async Task<HttpResponseMessage> GetSubTreeInternal(Guid id)
        {
            try
            {
                var res = await _DbSetManager.GetChildNodes(id, true);
                return Request.CreateResponse(HttpStatusCode.OK, res);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }
        }

        protected async Task<HttpResponseMessage> GetInternal<TRes>(Guid id) where TRes : T
        {
            try
            {
                var entity = await _DbSetManager.GetNode(id);
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
                await _DbSetManager.Create(entity, parentId);
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
                await _DbSetManager.Update(entity);
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
                await _DbSetManager.Move(id, parentId);
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
                var entity = await _DbSetManager.Entities.FirstOrDefaultAsync(e => e.Id == id);
                var parent = await _DbSetManager.GetParentNode(id);

                await _DbSetManager.Delete(entity);
                return Request.CreateResponse(HttpStatusCode.OK, parent);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }
    }
}
