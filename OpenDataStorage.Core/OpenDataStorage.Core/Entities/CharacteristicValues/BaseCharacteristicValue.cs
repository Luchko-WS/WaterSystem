using OpenDataStorage.Core.Attributes;
using OpenDataStorage.Core.Entities.NestedSets;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace OpenDataStorage.Core.Entities.CharacteristicValues
{
    public class BaseCharacteristicValue : BaseEntity
    {
        [IgnoreWhenUpdate]
        public Guid HierarchyObjectId { get; set; }

        public HierarchyObject HierarchyObject { get; set; }

        [IgnoreWhenUpdate]
        public Guid CharacteristicId { get; set; }

        public Characteristic Characteristic { get; set; }

        public string SubjectOfMonitoring { get; set; }

        public DateTime? EndCreationDate { get; set; }

        public bool IsTimeIntervalValue { get; set; }

        [NotMapped]
        public CharacteristicType ValueType { protected set; get; }
    }
}
