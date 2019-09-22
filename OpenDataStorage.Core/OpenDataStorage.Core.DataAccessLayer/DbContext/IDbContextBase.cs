using System.Threading.Tasks;

namespace OpenDataStorage.Core.DataAccessLayer.DbContext
{
    public interface IDbContextBase
    {
        Task SaveDbChangesAsync();
    }
}
