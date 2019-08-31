using System.Threading.Tasks;

namespace OpenDataStorage.Common.DbContext
{
    public interface IApplicationDbContextBase
    {
        Task SaveDbChangesAsync();
    }
}
