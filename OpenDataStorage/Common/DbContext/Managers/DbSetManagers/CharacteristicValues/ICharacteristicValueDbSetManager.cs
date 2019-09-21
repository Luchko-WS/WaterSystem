using OpenDataStorageCore.Entities.CharacteristicValues;
using System;
using System.Linq;

namespace OpenDataStorage.Common.DbContext.Managers.DbSetManagers.CharacteristicValues
{
    public interface ICharacteristicValueDbSetManager : IDbSetManager<BaseCharacteristicValue>
    {
        IQueryable<BaseCharacteristicValue> GetAllForCharacteristicQuery(Guid characteristicId, bool includeAll = true);

        IQueryable<BaseCharacteristicValue> GetAllForObjectQuery(Guid objectId, bool includeAll = true);
    }
}
