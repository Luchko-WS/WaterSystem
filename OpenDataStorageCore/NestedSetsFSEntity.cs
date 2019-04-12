﻿namespace OpenDataStorageCore
{
    public class NestedSetsFSEntity: NestedSetsEntity
    {
        public NestedSetsFSEntity() : base()
        { }

        public EntityType Type { get; set; }
    }

    public enum EntityType
    {
        Folder = 0,
        File = 1
    }
}
