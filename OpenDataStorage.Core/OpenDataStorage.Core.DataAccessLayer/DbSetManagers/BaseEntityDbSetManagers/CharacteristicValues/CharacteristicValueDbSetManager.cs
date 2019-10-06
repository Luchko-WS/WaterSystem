using OpenDataStorage.Common;
using OpenDataStorage.Core.DataAccessLayer.DbContext;
using OpenDataStorage.Core.Entities.CharacteristicValues;
using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace OpenDataStorage.Core.DataAccessLayer.DbSetManagers.BaseEntityDbSetManagers.CharacteristicValues
{
    public class CharacteristicValueDbSetManager<T> : BaseEntityDbSetManager<BaseCharacteristicValue>, ICharacteristicValueDbSetManager where T : BaseCharacteristicValue
    {
        public CharacteristicValueDbSetManager(DbSet<BaseCharacteristicValue> entities, IDbContextBase dbContext)
            : base(entities, dbContext, "CharacteristicValues") { }

        public IQueryable<BaseCharacteristicValue> GetAllForCharacteristicQueryWithAllDependencies(Guid characteristicId)
        {
            return base.GetAllQueryWithAllDependencies().Where(e => e.CharacteristicId == characteristicId);
        }

        public IQueryable<BaseCharacteristicValue> GetAllForCharacteristicQuery(Guid characteristicId, params Expression<Func<BaseCharacteristicValue, object>>[] includedPath)
        {
            return base.GetAllQuery(includedPath).Where(e => e.CharacteristicId == characteristicId);
        }

        public IQueryable<BaseCharacteristicValue> GetAllForObjectQueryWithAllDependencies(Guid objectId)
        {
            return base.GetAllQueryWithAllDependencies().Where(e => e.HierarchyObjectId == objectId);
        }

        public IQueryable<BaseCharacteristicValue> GetAllForObjectQuery(Guid objectId, params Expression<Func<BaseCharacteristicValue, object>>[] includedPath)
        {
            return base.GetAllQuery(includedPath).Where(e => e.HierarchyObjectId == objectId);
        }

        public override Task<Guid> CreateAsync(BaseCharacteristicValue entity)
        {
            ValidateDateInterval(entity);
            return base.CreateAsync(entity);
        }

        public override Task UpdateAsync(BaseCharacteristicValue entity)
        {
            ValidateDateInterval(entity);
            return base.UpdateAsync(entity);
        }

        protected override IQueryable<BaseCharacteristicValue> IncludeAllDependencies(IQueryable<BaseCharacteristicValue> query)
        {
            return query.Include(e => e.Characteristic).Include(e => e.HierarchyObject);
        }

        private void ValidateDateInterval(BaseCharacteristicValue value)
        {
            if (value.IsTimeIntervalValue)
            {
                if (!value.CreationDate.HasValue || !value.EndCreationDate.HasValue) throw new ArgumentException("Begin or end date value is missing for interval");
                if (value.CreationDate > value.EndCreationDate) throw new ArgumentException("Begin date cannot be greater than end date in interval");
            }
        }
    }
}