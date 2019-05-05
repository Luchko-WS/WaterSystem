using OpenDataStorageCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace OpenDataStorage.Common.DbContext
{
    public interface INestedSetsObjectContext<T> : INestedSetsEntityRelationContext<T> where T : NestedSetsEntity
    {
        IQueryable<T> Entities { get; }

        string TableName { get; }

        Task Add(T entity, Guid parentId);

        Task Update(T entity);

        Task Move(Guid entityId, Guid parentId);

        Task Remove(T entity);
    }
}
