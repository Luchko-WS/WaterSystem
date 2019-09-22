using OpenDataStorageCore.Entities.Aliases;

namespace OpenDataStorage.Common.DbContext.DbSetManagers.BaseEntityDbSetManagers.Aliases
{
    public interface IAliasDbSetManager<T> : IDbSetManager<T> where T: BaseAlias { }
}
