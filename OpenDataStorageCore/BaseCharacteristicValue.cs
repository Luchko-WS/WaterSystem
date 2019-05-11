using System;

namespace OpenDataStorageCore
{
    public class BaseCharacteristicValue : BaseEntity
    {
        public Guid HierarchyObjectId { get; set; }

        public HierarchyObject HierarchyObject { get; set; }

        public Guid CharacterisitcId { get; set; }

        public Characteristic Characteristic { get; set; }
    }
}
