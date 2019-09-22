using OpenDataStorageCore.Entities.NestedSets;
using System;

namespace OpenDataStorageCore.Entities.Aliases
{
    public class CharacteristicAlias : BaseAlias
    {
        public Guid CharacteristicId { get; set; }

        public virtual Characteristic Characteristic { get; set; }
    }
}
