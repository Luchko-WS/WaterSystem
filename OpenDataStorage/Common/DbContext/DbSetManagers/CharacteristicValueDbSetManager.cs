using OpenDataStorageCore.Entities.CharacteristicValues;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace OpenDataStorage.Common.DbContext.DbSetManagers
{
    public class CharacteristicValueDbSetManager<T> : BaseDbSetManager<BaseCharacteristicValue>, ICharacteristicValueDbSetManager where T : BaseCharacteristicValue
    {
        protected readonly Func<Task> _saveChangesFunction;
        public CharacteristicValueDbSetManager(DbSet<BaseCharacteristicValue> entities, Func<Task> SaveChangesFunction)
            : base(entities)
        {
            _saveChangesFunction = SaveChangesFunction;
            TableName = "CharacteristicValues";
        }

        public override IQueryable<BaseCharacteristicValue> GetEntityQuery(Guid id)
        {
            return IncludeDependencies(base.GetEntityQuery(id));
        }

        public override IQueryable<BaseCharacteristicValue> GetAllQuery()
        {
            return IncludeDependencies(base.GetAllQuery());
        }

        public IQueryable<BaseCharacteristicValue> GetAllForCharacteristicQuery(Guid characteristicId)
        {
            return IncludeDependencies(base.GetAllQuery()).Where(e => e.CharacterisitcId == characteristicId);
        }

        public IQueryable<BaseCharacteristicValue> GetAllForObjectQuery(Guid objectId)
        {
            return IncludeDependencies(base.GetAllQuery()).Where(e => e.HierarchyObjectId == objectId);
        }

        protected override async Task SaveChanges()
        {
            await _saveChangesFunction?.Invoke();
        }

        private IQueryable<BaseCharacteristicValue> IncludeDependencies(IQueryable<BaseCharacteristicValue> query)
        {
            return query.Include(e => e.Characteristic).Include(e => e.HierarchyObject);
        }
    }
}