using OpenDataStorage.Common.Attributes;
using OpenDataStorage.Core.DataAccessLayer.DbSetManagers.BaseEntityDbSetManagers.Aliases;
using OpenDataStorage.Helpers;
using OpenDataStorage.Core.Entities.Aliases;
using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace OpenDataStorage.API.Aliases
{
    [RoutePrefix("api/HierarchyObjectAliases")]
    [WebApiAuthorize(Roles = RolesHelper.DATA_MANAGEMENT_GROUP)]
    public class HierarchyObjectAliasesController : BaseAliasesController<HierarchyObjectAlias>
    {
        protected override IAliasDbSetManager<HierarchyObjectAlias> _DbSetManager => _dbContext.HierarchyObjectAliasDbSetManager;

        [AllowAnonymous]
        [Route("Get/{id}")]
        [HttpGet]
        public async Task<HierarchyObjectAlias> Get(Guid id)
        {
            return await base.GetInternal(id);
        }

        [AllowAnonymous]
        [Route("GetAllForObject/{id}")]
        [HttpGet]
        public async Task<dynamic> GetAliasesForObject(Guid id)
        {
            return await _DbSetManager.GetAllQueryWithAllDependencies()
                .Where(a => a.HierarchyObjectId == id)
                .OrderBy(a => a.Value)
                .ToListAsync();
        }

        [Route("Create/{objectId}")]
        [HttpPost]
        public async Task<HttpResponseMessage> Create([FromUri]Guid objectId, HierarchyObjectAlias vm)
        {
            var isObjectExist = await _dbContext.HierarchyObjectContext.Entities.AnyAsync(c => c.Id == objectId);
            if (isObjectExist)
            {
                var entity = new HierarchyObjectAlias
                {
                    HierarchyObjectId = objectId,
                    CreationDate = DateTime.Now.Date,
                    OwnerId = User.Identity.Name,
                    Value = vm.Value
                };
                return await base.CreateInternal(entity);
            }
            return Request.CreateErrorResponse(HttpStatusCode.NotFound, $"Object {objectId} not found");
        }

        [Route("Update")]
        [HttpPut]
        public async Task<HttpResponseMessage> Update(HierarchyObjectAlias entity)
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
