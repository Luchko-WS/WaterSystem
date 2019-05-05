using OpenDataStorageCore;

namespace OpenDataStorage.Common.DbContext
{
    public interface INestedSetsFSContext<T> : INestedSetsObjectContext<T> where T : NestedSetsFSEntity
    {
    }
}
