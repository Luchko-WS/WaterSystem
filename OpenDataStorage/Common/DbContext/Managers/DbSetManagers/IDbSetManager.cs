using OpenDataStorageCore.Entities;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace OpenDataStorage.Common.DbContext.Managers.DbSetManagers
{
    public interface IDbSetManager<T> where T : BaseEntity
    {
        string TableName { get; }

        IQueryable<T> GetEntityQuery(Guid id, bool includeAll = true);

        IQueryable<T> GetAllQuery(bool includeAll = true);

        Task Create(T entity);

        Task Update(T entity);

        Task Delete(Guid id);
    }
}
