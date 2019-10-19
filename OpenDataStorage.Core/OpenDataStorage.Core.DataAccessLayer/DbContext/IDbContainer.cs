using System.Data.Entity;

namespace OpenDataStorage.Core.DataAccessLayer.DbContext
{
    public interface IDbContainer
    {
        Database Database { get; }
    }
}
