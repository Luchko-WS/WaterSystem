using OpenDataStorage.Common.Attributes;
using OpenDataStorage.Helpers;
using OpenDataStorageCore.Constants;
using OpenDataStorageCore.Entities.CharacteristicValues;
using OpenDataStorageCore.Entities.NestedSets;
using SyncOpenDateServices.TextyOrgUaWater;
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
    [RoutePrefix("api/SystemManagement")]
    [WebApiAuthorize(Roles = IdentityConstants.Roles.SYSADMIN_ROLE)]
    public class SystemManagementController : BaseApiController
    {
        [Route("SyncWithTextyOrgUaWaterService")]
        [HttpPost]
        [WebApiAuthorize(Roles = RolesHelper.DATA_SYNC_GROUP)]
        public async Task<HttpResponseMessage> SyncWithTextyOrgUaWaterService()
        {
            var service = new TextyOrgUaWaterService();
            var data = await service.GetData();

            //characteristics
            var characteristicMap = new Dictionary<string, Guid>();
            var rootCharacteristic = await _dbContext.CharacteristicContext.Entities.SingleAsync(o => o.Level == 0);
            var characteristicsNames = data.Select(i => i.Key).Distinct(StringComparer.InvariantCultureIgnoreCase);
            foreach(var characteristicName in characteristicsNames)
            {
                await _dbContext.ReloadFromDb(rootCharacteristic);
                Guid characteristicId;
                var characteristic = await _dbContext.CharacteristicContext.Entities.FirstOrDefaultAsync(o => o.Name == characteristicName);
                if (characteristic == null)
                {
                    var entity = new Characteristic
                    {
                        Name = characteristicName,
                        Description = "synced from TextyOrgUaWaterService",
                        OwnerId = User.Identity.Name,
                        EntityType = EntityType.File,
                        CharacteristicType = CharacteristicType.Number
                    };
                    await _dbContext.CharacteristicContext.Add(entity, rootCharacteristic.Id);
                    characteristicId = entity.Id;
                }
                else
                {
                    characteristicId = characteristic.Id;
                }

                characteristicMap.Add(characteristicName, characteristicId);
            }

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
            foreach(var river in objects)
            {
                await _dbContext.ReloadFromDb(rootObject);
                Guid riverId;
                var riverObj = await _dbContext.HierarchyObjectContext.Entities.FirstOrDefaultAsync(o => o.Name == river.River);
                if (riverObj == null)
                {
                    var entity = new HierarchyObject
                    {
                        Name = river.River,
                        Description = "synced by TextyOrgUaWaterService",
                        OwnerId = User.Identity.Name,
                        ObjectTypeId = null //river
                    };
                    riverId = await _dbContext.HierarchyObjectContext.Add(entity, rootObject.Id);
                }
                else
                {
                    riverId = riverObj.Id;
                }

                var riverObject = await _dbContext.HierarchyObjectContext.Entities.FirstOrDefaultAsync(o => o.Id == riverId);
                foreach(var point in river.Points)
                {
                    await _dbContext.ReloadFromDb(riverObject);
                    Guid pointId;
                    var pointObj = await _dbContext.HierarchyObjectContext.Entities.FirstOrDefaultAsync(o => o.Name == point.Name);
                    if (pointObj == null)
                    {
                        var entity = new HierarchyObject
                        {
                            Name = point.Name,
                            Description = $"Code: {point.Info.Code}. Lab: {point.Info.Laboratory}. Synced by TextyOrgUaWaterService", //save in different db field
                            OwnerId = User.Identity.Name,
                            ObjectTypeId = null //point
                        };
                        await _dbContext.HierarchyObjectContext.Add(entity, riverId);
                        pointId = entity.Id;
                    }
                    else
                    {
                        pointId = pointObj.Id;
                    }

                    foreach (var value in point.Values)
                    {
                        var entity = new NumberCharacteristicValue
                        {
                            CharacterisitcId = characteristicMap[value.Key],
                            HierarchyObjectId = pointId,
                            CreationDate = value.Date,
                            Value = value.Value,
                            OwnerId = User.Identity.Name,
                            SubjectOfMonitoring = SubjectOfMonitoringConstants.CLEAR_WATER
                            //value.Id -- save in different field
                        };

                        await _dbContext.CharacteristicValueDbSetManager.Create(entity);
                    }
                }
            }

            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}
