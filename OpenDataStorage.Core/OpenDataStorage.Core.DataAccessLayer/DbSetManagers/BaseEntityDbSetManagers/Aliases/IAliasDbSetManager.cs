using OpenDataStorage.Core.Entities.Aliases;

namespace OpenDataStorage.Core.DataAccessLayer.DbSetManagers.BaseEntityDbSetManagers.Aliases
{
    public interface IAliasDbSetManager<T> : IDbSetManager<T> where T: BaseAlias { }
}
