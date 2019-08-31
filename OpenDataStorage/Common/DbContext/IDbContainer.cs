using System.Data.Entity;

namespace OpenDataStorage.Common.DbContext
{
    public interface IDbContainer
    {
        Database Database { get; }
    }
}
