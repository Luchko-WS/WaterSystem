using OpenDataStorageCore;
using System.Linq;
using System.Threading.Tasks;

namespace OpenDataStorage.Common.DbContext
{
    public interface IApplicationDbContext
    {
        IQueryable<HierarchyObject> HierarchyObjects { get; }

        IQueryable<Characteristic> Characteristics { get; }

        Task SaveDbChangesAsync();
    }
}
