using OpenDataStorageCore.Entities.NestedSets;
using System;

namespace OpenDataStorageCore.Entities.Aliases
{
    public class CharacteristicAlias : BaseAlias
    {
        public Guid CharacteristicId { get; set; }

        public Characteristic Characteristic { get; set; }
    }
}
