using OpenDataStorage.Core.Entities.CharacteristicValues;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace OpenDataStorage.Core.DataAccessLayer.DbSetManagers.BaseEntityDbSetManagers.CharacteristicValues
{
    public interface ICharacteristicValueDbSetManager : IDbSetManager<BaseCharacteristicValue>
    {
        IQueryable<BaseCharacteristicValue> GetAllForCharacteristicQueryWithAllDependencies(Guid characteristicId);

        IQueryable<BaseCharacteristicValue> GetAllForCharacteristicQuery(Guid characteristicId, params Expression<Func<BaseCharacteristicValue, object>>[] includedPath);

        IQueryable<BaseCharacteristicValue> GetAllForObjectQueryWithAllDependencies(Guid objectId);

        IQueryable<BaseCharacteristicValue> GetAllForObjectQuery(Guid objectId, params Expression<Func<BaseCharacteristicValue, object>>[] includedPath);
    }
}
