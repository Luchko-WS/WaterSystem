﻿using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace OpenDataStorageCore
{
    public class BaseCharacteristicValue : BaseEntity
    {
        public Guid HierarchyObjectId { get; set; }

        public HierarchyObject HierarchyObject { get; set; }

        public Guid CharacterisitcId { get; set; }

        public Characteristic Characteristic { get; set; }

        [NotMapped]
        public CharacteristicType ValueType { protected set; get; }
    }
}
