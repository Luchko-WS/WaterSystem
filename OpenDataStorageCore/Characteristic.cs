using System.Collections.Generic;

namespace OpenDataStorageCore
{
    public class Characteristic : NestedSetsFSEntity
    {
        public Characteristic() : base() { }

        public string Name { get; set; }

        public string Description { get; set; }

        public ICollection<CharacteristicValue> CharacteristicValues { get; set; }
    }
}
