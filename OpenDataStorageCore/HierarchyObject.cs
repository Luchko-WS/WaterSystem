using System.Collections.Generic;

namespace OpenDataStorageCore
{
    public class HierarchyObject : NestedSetsEntity
    {
        public HierarchyObject() : base() { }

        public string Name { get; set; }

        public string Description { get; set; }

        //public Guid HierarchyObjectTypeId { get; set; }

        //public ObjectType ObjectType { get; set; }

        public ICollection<CharacteristicValue> CharacteristicValues { get; set; }
    }
}
