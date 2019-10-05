﻿namespace OpenDataStorageCore.Entities.NestedSets
{
    public class NestedSetsFSEntity: NestedSetsEntity
    {
        public NestedSetsFSEntity() : base()
        { }

        public EntityType EntityType { get; set; }
    }

    public enum EntityType
    {
        Folder = 0,
        File = 1
    }
}