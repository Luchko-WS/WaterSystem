using System.Collections.Generic;

namespace OpenDataStorageCore
{
    public class HierarchyObject : NestedSetsEntity
    {
        public HierarchyObject()
        {
            this.Characteristics = new List<Characteristic>();
        }

        public string Name { get; set; }

        public string Description { get; set; }

        public ICollection<Characteristic> Characteristics { get; set; }
    }
}
