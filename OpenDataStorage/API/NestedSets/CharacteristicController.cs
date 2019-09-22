using OpenDataStorage.Common;
using OpenDataStorage.Common.Attributes;
using OpenDataStorage.Common.DbContext.DbSetManagers.NestedSetsEntityManagers.Core;
using OpenDataStorage.Helpers;
using OpenDataStorage.ViewModels.CharacteristicViewModel;
using OpenDataStorageCore.Entities.NestedSets;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace OpenDataStorage.API.NestedSets
{
    [RoutePrefix("api/Characteristics")]
    [WebApiAuthorize(Roles = RolesHelper.DATA_MANAGEMENT_GROUP)]
    public class CharacteristicController : BaseNestedSetsController<Characteristic>
    {
        protected override INestedSetsDbSetManager<Characteristic> _DbSetManager => _dbContext.CharacteristicContext;

        [Route("GetTree")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<ICollection<CharacteristicViewModel>> GetTree([FromUri]CharacteristicFilterViewModel vm)
        {
            if (vm != null && !string.IsNullOrEmpty(vm.Name))
            {
                var ids = await _dbContext.CharacteristicContext.Entities
                    .Include(e => e.CharacteristicAliases)
                    .Where(e => e.Name.ToLower().Contains(vm.Name.ToLower()) ||
                        e.CharacteristicAliases.Any(a => a.Value.ToLower().Contains(vm.Name.ToLower())))
                    .Select(e => e.Id).ToListAsync();

                var results = new List<Characteristic>();
                foreach (var id in ids)
                {
                    if (!results.Any(e => e.Id == id))
                    {
                        var branch = await _dbContext.CharacteristicContext.GetParentsWithAllDependencies(id, true);
                        results = results.Union(branch).ToList();
                    }
                }
                return results.OrderBy(e => e.LeftKey).Select(e => Mapper.CreateInstanceAndMapProperties<CharacteristicViewModel>(e)).ToList();
            }

            var res = await _dbContext.CharacteristicContext.GetAllQueryWithAllDependencies().ToListAsync();
            return res.Select(e => Mapper.CreateInstanceAndMapProperties<CharacteristicViewModel>(e)).ToList();
        }

        [Route("GetSubTree")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<HttpResponseMessage> GetSubTree([FromUri]CharacteristicViewModel vm)
        {
            return await GetSubTreeInternal(vm.Id,
                e => e.CharacteristicAliases);
        }

        [Route("Get/{id}")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<HttpResponseMessage> Get([FromUri]Guid id)
        {
            return await GetInternal<CharacteristicViewModel>(id,
                e => e.CharacteristicAliases);
        }

        [Route("Create/{parentFolderId}")]
        [HttpPost]
        public async Task<HttpResponseMessage> Create([FromUri]Guid parentFolderId, CharacteristicViewModel vm)
        {
            var entity = new Characteristic
            {
                Name = vm.Name,
                Description = vm.Description,
                EntityType = vm.EntityType,
                CharacteristicType = vm.EntityType == EntityType.Folder ? CharacteristicType.None : vm.CharacteristicType,
                OwnerId = User.Identity.Name
            };
            return await CreateInternal(entity, parentFolderId);
        }

        [Route("Update")]
        [HttpPut]
        public async Task<HttpResponseMessage> Update(CharacteristicViewModel vm)
        {
            var entity = Mapper.CreateInstanceAndMapProperties<Characteristic>(vm);
            return await UpdateInternal(entity);
        }

        [Route("Move/{id}/{parentId}")]
        [HttpPost]
        public async Task<HttpResponseMessage> Move([FromUri]Guid id, Guid parentId)
        {
            return await MoveInternal(id, parentId);
        }

        [Route("Delete/{id}")]
        [HttpDelete]
        public async Task<HttpResponseMessage> Delete(Guid id)
        {
            return await DeleteInternal(id);
        }
    }
}
