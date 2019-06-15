using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using OpenDataStorageCore.Entities.Aliases;

namespace OpenDataStorage.Common.DbContext.DbSetManagers.Aliases
{
    public class CharacteristicAliasDbSetManager : BaseDbSetManager<CharacteristicAlias>, ICharacteristicAliasDbSetManager
    {
        protected readonly Func<Task> _saveChangesFunction;
        public CharacteristicAliasDbSetManager(DbSet<CharacteristicAlias> entities, Func<Task> SaveChangesFunction)
            : base(entities)
        {
            _saveChangesFunction = SaveChangesFunction;
            TableName = "CharacteristicAliases";
        }

        protected override async Task SaveChanges()
        {
            await _saveChangesFunction?.Invoke();
        }

        protected override IQueryable<CharacteristicAlias> IncludeDependencies(IQueryable<CharacteristicAlias> query)
        {
            return query.Include(e => e.Characteristic);
        }
    }
}
