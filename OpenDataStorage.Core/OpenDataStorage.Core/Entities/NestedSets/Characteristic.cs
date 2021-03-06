﻿using OpenDataStorage.Core.Attributes;
using OpenDataStorage.Core.Entities.Aliases;
using System.Collections.Generic;

namespace OpenDataStorage.Core.Entities.NestedSets
{
    public class Characteristic : NestedSetsFSEntity
    {
        public Characteristic() : base() { }

        public string Name { get; set; }

        public string Description { get; set; }

        public virtual ICollection<CharacteristicAlias> CharacteristicAliases { get; set; }

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
