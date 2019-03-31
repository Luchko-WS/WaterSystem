using OpenDataStorage.Common;
using OpenDataStorage.ViewModels.HierarchyObjectViewModels;
using OpenDataStorageCore;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace OpenDataStorage.API
{
    [RoutePrefix("api/HierarchyObjects")]
    public class HierarchyObjectController : BaseApiController
    {
        [Route("GetHierarchyObjectTree")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<dynamic> GetHierarchyObjectTree()
        {
            //test
            var collection = _dbContext.HierarchyObjectContext.Entities;
            return collection;
        }

        [Route("GetHierarchyObjectSubTree")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<dynamic> GetHierarchyObjectSubTree([FromUri]HierarchyObjectViewModel filter)
        {
            throw new NotImplementedException();
        }

        [Route("HierarchyObject/{HierarchyObjectId}")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<HttpResponseMessage> GetHierarchyObject([FromUri]Guid HierarchyObjectId)
        {
            throw new NotImplementedException();
        }

        [Route("CreateHierarchyObject")]
        [HttpPost]
        public async Task<HttpResponseMessage> CreateHierarchyObject(HierarchyObjectViewModel vm)
        {
            //test
            var parentFolder = _dbContext.HierarchyObjectContext.Entities.Where(e => e.Type == EntityType.Folder).FirstOrDefault();

            var HierarchyObject = new HierarchyObject
            {
                Name = vm.Name,
                Description = vm.Description,
                Type = EntityType.File,
                OwnerId = User.Identity.Name
            };
            await _dbContext.HierarchyObjectContext.AddObject(HierarchyObject, parentFolder.Id);
            return Request.CreateResponse(HttpStatusCode.OK, HierarchyObject);
        }

        [Route("Edit/{HierarchyObjectId}")]
        [HttpPut]
        public async Task<HttpResponseMessage> EditHierarchyObject([FromUri]Guid HierarchyObjectId, HierarchyObjectViewModel vm)
        {
            var HierarchyObject = Mapper.CreateInstanceAndMapProperties<HierarchyObject>(vm);
            await _dbContext.HierarchyObjectContext.UpdateObject(HierarchyObject);
            return Request.CreateResponse(HttpStatusCode.OK, HierarchyObject);
        }

        [Route("Remove/{id}")]
        [HttpDelete]
        public async Task<HttpResponseMessage> RemoveHierarchyObject(Guid id)
        {
            await _dbContext.HierarchyObjectContext.RemoveObject(id);
            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}
