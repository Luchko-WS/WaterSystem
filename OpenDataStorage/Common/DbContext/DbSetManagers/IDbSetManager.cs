using OpenDataStorageCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace OpenDataStorage.Common.DbContext.DbSetManagers
{
    public interface IDbSetManager<T> where T : BaseEntity
    {
        string TableName { get; }

        IQueryable<T> GetEntityQuery(Guid id);

        IQueryable<T> GetAllQuery();

        Task Create(T entity);

        Task Update(T entity);

        Task Delete(Guid id);
    }
}
