using OpenDataStorage.Core.Attributes;
using OpenDataStorage.Core.Entities.NestedSets;
using System;

namespace OpenDataStorage.Core.Entities.Aliases
{
    public class CharacteristicAlias : BaseAlias
    {
        [IgnoreWhenUpdate]
        public Guid CharacteristicId { get; set; }

        public virtual Characteristic Characteristic { get; set; }
    }
}
