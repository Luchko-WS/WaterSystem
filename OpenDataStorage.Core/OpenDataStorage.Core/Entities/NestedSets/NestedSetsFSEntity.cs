﻿using OpenDataStorage.Core.Attributes;

namespace OpenDataStorage.Core.Entities.NestedSets
{
    public class NestedSetsFSEntity: NestedSetsEntity
    {
        public NestedSetsFSEntity() : base()
        { }

        [IgnoreWhenUpdate]
        public EntityType EntityType { get; set; }
    }

    public enum EntityType
    {
        Folder = 0,
        File = 1
    }
}
