using OpenDataStorage.ViewModels.ReportsViewModel;
using OpenDataStorageCore;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace OpenDataStorage.API
{
    [RoutePrefix("api/Reports")]
    public class ReportsController : BaseApiController
    {
        [Route("Generate")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<dynamic> Generate([FromUri]ReportViewModel vm)
        {
            if (vm != null && vm.FromDate.HasValue && vm.ToDate.HasValue && vm.FromDate > vm.ToDate)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "FromDate must by less than ToDate");
            }

            ICollection<ObjectType> types = null;
            if(vm != null && vm.TypeId.HasValue)
            {
                types = await _dbContext.ObjectTypeContext.GetChildNodes(vm.TypeId.Value, true);
            }

            ICollection<HierarchyObject> objects = null;
            if (vm != null && vm.ObjectId.HasValue)
            {
                objects = await _dbContext.HierarchyObjectContext.GetChildNodes(vm.ObjectId.Value, true);
            }
            else
            {
                objects = await _dbContext.HierarchyObjectContext.GetTree();
            }

            if (types != null && types.Any())
            {
                objects = objects.Where(o => types.FirstOrDefault(t => t.Id == o.ObjectTypeId) != null).ToList();
            }

            ICollection<Characteristic> characteristics = null;
            if(vm != null && vm.CharacterisitcId.HasValue)
            {
                characteristics = await _dbContext.CharacteristicContext.GetChildNodes(vm.CharacterisitcId.Value, true);
            }

            ICollection<BaseCharacteristicValue> values = await _dbContext.CharacteristicValueDbSetManager.GetAllQuery().ToListAsync();
            ICollection<NumberCharacteristicValue> data = values
                .Where(v => v.ValueType == CharacteristicType.Number && objects.FirstOrDefault(o => o.Id == v.HierarchyObjectId) != null)
                .Cast<NumberCharacteristicValue>().ToList();
            if(characteristics != null && characteristics.Any())
            {
                data = data.Where(v => characteristics.FirstOrDefault(c => c.Id == v.CharacterisitcId) != null).ToList();
            }

            if (vm != null)
            {
                if (vm.FromDate.HasValue) data = data.Where(v => v.CreationDate >= vm.FromDate.Value).ToList();
                if (vm.ToDate.HasValue) data = data.Where(v => v.CreationDate <= vm.ToDate.Value).ToList();
            }

            return data.GroupBy(i => i.HierarchyObjectId)
                .Select(g1 => new
                {
                    Object = g1.FirstOrDefault()?.HierarchyObject,
                    Characteristics = g1.GroupBy(c => c.CharacterisitcId)
                        .Select(g2 => new
                        {
                            Characteristic = g2.FirstOrDefault()?.Characteristic,
                            Data = g2.OrderBy(v => v.CreationDate).GroupBy(v => v.CreationDate.Value.Date)
                                .Select(group3 => new
                                {
                                    Date = group3.Key,
                                    Value = group3.Average(v => v.Value)
                                })
                        })
                }).OrderBy(i => i.Object.LeftKey);
        }
    }
}