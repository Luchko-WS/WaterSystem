using System.Collections.Generic;

namespace OpenDataStorageCore
{
    public class HierarchyObject : NestedSetsObject
    {
        public HierarchyObject() : base() { }

        public ICollection<CharacteristicValue> CharacteristicValues { get; set; }
    }
}
