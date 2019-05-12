using OpenDataStorageCore;

namespace OpenDataStorage.Common.DbContext.NestedSets
{
    public interface INestedSetsFSContext<T> : INestedSetsObjectContext<T> where T : NestedSetsFSEntity
    {
    }
}
