using OpenDataStorage.Common;
using OpenDataStorage.ViewModels.ObjectTypeViewModels;
using OpenDataStorageCore;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace OpenDataStorage.API
{
    [RoutePrefix("api/ObjectType")]
    public class ObjectTypeController : BaseApiController
    {
        [Route("GetTree")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<ICollection<ObjectType>> GetTree([FromUri]ObjectTypeFilterViewModel vm)
        {
            if (vm != null && !string.IsNullOrEmpty(vm.Name))
            {
                var ids = await _dbContext.ObjectTypeContext.Entities
                    .Where(e => e.Name.ToLower().Contains(vm.Name.ToLower()))
                    .Select(e => e.Id).ToListAsync();

                var results = new List<ObjectType>();
                foreach(var id in ids)
                {
                    if (!results.Any(e => e.Id == id))
                    {
                        var branch = await _dbContext.ObjectTypeContext.GetParentNodes(id, includeItself: true);
                        results = results.Union(branch).ToList();
                    }
                }
                return results.OrderBy(e => e.LeftKey).ToList();
            }
            return await _dbContext.ObjectTypeContext.GetTree();
        }

        [Route("GetSubTree")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<HttpResponseMessage> GetSubTree([FromUri]ObjectTypeViewModel vm)
        {
            try
            {
                var res = await _dbContext.ObjectTypeContext.GetChildNodes(vm.Id, true);
                return Request.CreateResponse(HttpStatusCode.OK, res);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }
        }

        [Route("Get/{id}")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<HttpResponseMessage> Get([FromUri]Guid id)
        {
            try
            {
                var res = await _dbContext.ObjectTypeContext.GetNode(id);
                return Request.CreateResponse(HttpStatusCode.OK, res);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }
        }

        [Route("Create/{parentFolderId}")]
        [HttpPost]
        public async Task<HttpResponseMessage> Create([FromUri]Guid parentFolderId, ObjectTypeViewModel vm)
        {
            try
            {
                var entity = new ObjectType
                {
                    Name = vm.Name,
                    Description = vm.Description,
                    Type = vm.Type,
                    OwnerId = User.Identity.Name
                };

                await _dbContext.ObjectTypeContext.Add(entity, parentFolderId);
                return Request.CreateResponse(HttpStatusCode.OK, entity);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        [Route("Update")]
        [HttpPut]
        public async Task<HttpResponseMessage> Update(ObjectTypeViewModel vm)
        {
            try
            {
                var entity = Mapper.CreateInstanceAndMapProperties<ObjectType>(vm);
                await _dbContext.ObjectTypeContext.Update(entity);
                return Request.CreateResponse(HttpStatusCode.OK, entity);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        [Route("Move")]
        [HttpPut]
        public async Task<HttpResponseMessage> Move(Guid id, Guid parentId)
        {
            try
            {
                var entity = _dbContext.ObjectTypeContext.Entities.FirstOrDefault(e => e.Id == id);
                await _dbContext.ObjectTypeContext.Move(id, parentId);
                return Request.CreateResponse(HttpStatusCode.OK, entity);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        [Route("Delete/{id}")]
        [HttpDelete]
        public async Task<HttpResponseMessage> Delete(Guid id)
        {
            try
            {
                var entity = await _dbContext.ObjectTypeContext.Entities.FirstOrDefaultAsync(e => e.Id == id);
                var parent = await _dbContext.ObjectTypeContext.GetParentNode(id);

                await _dbContext.ObjectTypeContext.Remove(entity);
                return Request.CreateResponse(HttpStatusCode.OK, parent);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }
    }
}
