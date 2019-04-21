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

        Task AddObject(T @object, Guid parentId);

        Task UpdateObject(T @object);

        Task MoveObject(T @object, Guid parentId);

        Task RemoveObject(T @object);
    }
}
