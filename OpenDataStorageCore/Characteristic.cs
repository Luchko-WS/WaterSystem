using System.Collections.Generic;

namespace OpenDataStorageCore
{
    public class Characteristic : NestedSetsEntity
    {
        public Characteristic()
        {
            this.Objects = new List<HierarchyObject>();
        }

        public string Name { get; set; }

        public string Description { get; set; }

        public ICollection<HierarchyObject> Objects { get; set; }
    }
}
