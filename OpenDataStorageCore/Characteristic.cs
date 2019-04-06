using System.Collections.Generic;

namespace OpenDataStorageCore
{
    public class Characteristic : NestedSetsObject
    {
        public Characteristic() : base() { }

        public ICollection<CharacteristicValue> CharacteristicValues { get; set; }
    }
}
