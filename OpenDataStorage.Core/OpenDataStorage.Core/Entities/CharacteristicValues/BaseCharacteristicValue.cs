using OpenDataStorageCore.Entities.NestedSets;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace OpenDataStorageCore.Entities.CharacteristicValues
{
    public class BaseCharacteristicValue : BaseEntity
    {
        public Guid HierarchyObjectId { get; set; }

        public HierarchyObject HierarchyObject { get; set; }

        public Guid CharacteristicId { get; set; }

        public Characteristic Characteristic { get; set; }

        public string SubjectOfMonitoring { get; set; }

        public DateTime? EndCreationDate { get; set; }

        public bool IsTimeIntervalValue { get; set; }

        [NotMapped]
        public CharacteristicType ValueType { protected set; get; }
    }
}
