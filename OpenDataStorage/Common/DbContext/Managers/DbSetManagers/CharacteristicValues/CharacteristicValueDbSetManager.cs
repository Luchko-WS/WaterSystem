using OpenDataStorageCore.Entities.CharacteristicValues;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace OpenDataStorage.Common.DbContext.Managers.DbSetManagers.CharacteristicValues
{
    public class CharacteristicValueDbSetManager<T> : BaseDbSetManager<BaseCharacteristicValue>, ICharacteristicValueDbSetManager where T : BaseCharacteristicValue
    {
        protected readonly Func<Task> _saveChangesFunction;
        public CharacteristicValueDbSetManager(DbSet<BaseCharacteristicValue> entities, IApplicationDbContextBase dbContext)
            : base(entities, dbContext, "CharacteristicValues") { }

        public IQueryable<BaseCharacteristicValue> GetAllForCharacteristicQuery(Guid characteristicId, bool includeAll = true)
        {
            return base.GetAllQuery(includeAll).Where(e => e.CharacteristicId == characteristicId);
        }

        public IQueryable<BaseCharacteristicValue> GetAllForObjectQuery(Guid objectId, bool includeAll = true)
        {
            return base.GetAllQuery(includeAll).Where(e => e.HierarchyObjectId == objectId);
        }

        public override Task Create(BaseCharacteristicValue entity)
        {
            ValidateDateInterval(entity);
            return base.Create(entity);
        }

        public override Task Update(BaseCharacteristicValue entity)
        {
            ValidateDateInterval(entity);
            return base.Update(entity);
        }

        protected override IQueryable<BaseCharacteristicValue> IncludeDependencies(IQueryable<BaseCharacteristicValue> query)
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