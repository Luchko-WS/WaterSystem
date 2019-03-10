using OpenDataStorageCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenDataStorage.Common
{
    public interface IApplicationDbContext
    {
        IQueryable<HierarchyObject> HierarchyObjects { get; }

        IQueryable<Characteristic> Characteristics { get; }

        Task SaveDbChangesAsync();
    }
}
