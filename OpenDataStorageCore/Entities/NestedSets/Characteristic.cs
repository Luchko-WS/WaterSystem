using OpenDataStorageCore.Attributes;
using OpenDataStorageCore.Entities.Aliases;
using System.Collections.Generic;

namespace OpenDataStorageCore.Entities.NestedSets
{
    public class Characteristic : NestedSetsFSEntity
    {
        public Characteristic() : base() { }

        public string Name { get; set; }

        public string Description { get; set; }

        public ICollection<CharacteristicAlias> CharacteristicAliases { get; set; }

        [IgnoreWhenUpdate]
        public CharacteristicType CharacteristicType { get; set; }
    }

    public enum CharacteristicType
    {
        None = 0,
        Number = 1,
        String = -2
    }
}
