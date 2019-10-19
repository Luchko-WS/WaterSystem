using OpenDataStorage.Common.Attributes;
using OpenDataStorage.Helpers;
using OpenDataStorage.Core.Constants;
using OpenDataStorage.Core.Entities.CharacteristicValues;
using OpenDataStorage.Core.Entities.NestedSets;
using SyncOpenDateServices.SacmigFormat;
using SyncOpenDateServices.TextyOrgUaWater;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace OpenDataStorage.API.Management
{
    [RoutePrefix("api/DataSynch")]
    [WebApiAuthorize(Roles = RolesHelper.DATA_SYNC_GROUP)]
    public class DataSynchManagementController : BaseApiController
    {
        [Route("SyncWithTextyOrgUaWaterService")]
        [HttpPost]
        public async Task<HttpResponseMessage> SyncWithTextyOrgUaWaterService()
        {
            var service = new TextyOrgUaWaterService();
            var data = await service.GetData();

            using (var transaction = _dbContext.BeginTransaction())
            {
                try
                {
                    //characteristics
                    var characteristicsNames = data.Select(i => i.Key).Distinct(StringComparer.InvariantCultureIgnoreCase);
                    var serviceDescription = "Synced by TextyOrgUaWaterService";
                    var characteristicMap = await MapCharateristicsNamesAndCreateWhetherDoesNotExist(characteristicsNames, serviceDescription);

                    //objects
                    var objects = data.GroupBy(i => i.River, StringComparer.InvariantCultureIgnoreCase)
                        .Select(group => new
                        {
                            Points = group.GroupBy(p => p.Name, StringComparer.InvariantCultureIgnoreCase)
                                .Select(group2 => new
                                {
                                    Name = group2.Key,
                                    Info = new
                                    {
                                        Code = group2.FirstOrDefault().Code,
                                        Laboratory = group2.FirstOrDefault().Laboratory
                                    },
                                    Values = group2.Select(v => new
                                    {
                                        Id = v.Id,
                                        Date = v.Date,
                                        Key = v.Key,
                                        Value = v.Value
                                    })
                                }),
                            River = group.Key
                        });

                    var rootObject = await _dbContext.HierarchyObjectContext.Entities.SingleAsync(o => o.Level == 0);
                    foreach (var river in objects)
                    {
                        Guid riverId = await GetObjectIdAndCreateWhetherDoesNotExist(river.River, serviceDescription, rootObject);
                        var riverObject = await _dbContext.HierarchyObjectContext.Entities.FirstOrDefaultAsync(o => o.Id == riverId);

                        foreach (var point in river.Points)
                        {
                            Guid pointId = await GetObjectIdAndCreateWhetherDoesNotExist(point.Name, serviceDescription, riverObject);
                            foreach (var value in point.Values)
                            {
                                var entity = new NumberCharacteristicValue
                                {
                                    CharacteristicId = characteristicMap[value.Key],
                                    HierarchyObjectId = pointId,
                                    CreationDate = value.Date,
                                    Value = value.Value,
                                    OwnerId = User.Identity.Name,
                                    SubjectOfMonitoring = SubjectOfMonitoringConstants.CLEAR_WATER
                                };
                                await _dbContext.CharacteristicValueDbSetManager.CreateAsync(entity);
                            }
                        }
                    }

                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                }
            }

            return Request.CreateResponse(HttpStatusCode.Created);
        }

        [Route("UploadSacmigDataFile/{objectId}")]
        [HttpPost]
        public async Task<HttpResponseMessage> UploadDataFile([FromUri]Guid objectId)
        {
            var obj = await _dbContext.HierarchyObjectContext.Entities.FirstOrDefaultAsync(o => o.Id == objectId);
            if (obj == null) return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Object not found");

            if (HttpContext.Current.Request.Files.Count > 0)
            {
                foreach (string fileName in HttpContext.Current.Request.Files)
                {
                    var file = HttpContext.Current.Request.Files[fileName];
                    if (file.ContentType == "application/vnd.ms-excel" || 
                        file.ContentType == "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                    {
                        try
                        {
                            var parser = new SacmigFileParser();
                            var data = parser.Parse(file.InputStream);

                            //check characteristics
                            var characteristicsNames = data.GetCharacteristiNames();
                            var serviceDescription = "synced from Sacmig file format";
                            var characteristicsMap = await MapCharateristicsNamesAndCreateWhetherDoesNotExist(characteristicsNames, serviceDescription);

                            using (var transaction = _dbContext.BeginTransaction())
                            {
                                try
                                {
                                    //save values
                                    foreach (var characteristic in characteristicsNames)
                                    {
                                        foreach (var value in data.GetCharacteristicValues(characteristic))
                                        {
                                            value.CharacteristicId = characteristicsMap[characteristic];
                                            value.HierarchyObjectId = objectId;
                                            value.OwnerId = User.Identity.Name;
                                            await _dbContext.CharacteristicValueDbSetManager.CreateAsync(value);
                                        }
                                    }
                                    transaction.Commit();
                                }
                                catch
                                {
                                    transaction.Rollback();
                                }
                            }
                        }
                        catch
                        {
                            continue;
                        }
                    }
                }
            }
            return Request.CreateResponse(HttpStatusCode.OK);
        }

        private async Task<Guid> GetObjectIdAndCreateWhetherDoesNotExist(string objectName, string serviceDescription, HierarchyObject rootObject = null)
        {
            var obj = await _dbContext.HierarchyObjectContext.Entities
                .Include(o => o.HierarchyObjectAliases)
                .FirstOrDefaultAsync(o => o.Name.ToLower() == objectName.ToLower() ||
                    o.HierarchyObjectAliases.FirstOrDefault(a => a.Value.ToLower() == objectName.ToLower()) != null);

            if (obj == null)
            {
                if (rootObject == null)
                {
                    rootObject = await _dbContext.HierarchyObjectContext.Entities.SingleAsync(o => o.Level == 0);
                }

                await _dbContext.ReloadEntityFromDb(rootObject);
                var entity = new HierarchyObject
                {
                    Name = objectName,
                    Description = serviceDescription,
                    OwnerId = User.Identity.Name,
                    ObjectTypeId = null
                };
                return await _dbContext.HierarchyObjectContext.CreateAsync(entity, rootObject.Id);
            }
            else
            {
                return obj.Id;
            }
        }

        private async Task<Dictionary<string, Guid>> MapCharateristicsNamesAndCreateWhetherDoesNotExist(IEnumerable<string> characteristicsNames, 
            string serviceDescription, Characteristic rootCharacteristic = null)
        {
            var characteristicMap = new Dictionary<string, Guid>();

            foreach (var characteristicName in characteristicsNames)
            {
                var characteristic = await _dbContext.CharacteristicContext.Entities
                       .Include(c => c.CharacteristicAliases)
                       .FirstOrDefaultAsync(c => c.Name.ToLower() == characteristicName.ToLower() ||
                           c.CharacteristicAliases.Any(a => a.Value.ToLower() == characteristicName.ToLower()));

                if (characteristic == null)
                {
                    if (rootCharacteristic == null)
                    {
                        rootCharacteristic = await _dbContext.CharacteristicContext.Entities.SingleAsync(o => o.Level == 0);
                    }
                    await _dbContext.ReloadEntityFromDb(rootCharacteristic);

                    var entity = new Characteristic
                    {
                        Name = characteristicName,
                        Description = serviceDescription,
                        OwnerId = User.Identity.Name,
                        EntityType = EntityType.File,
                        CharacteristicType = CharacteristicType.Number
                    };
                    characteristicMap[characteristicName] = await _dbContext.CharacteristicContext.CreateAsync(entity, rootCharacteristic.Id);
                }
                else
                {
                    characteristicMap[characteristicName] = characteristic.Id;
                }
            }
            return characteristicMap;
        }
    }
}
