using OpenDataStorageCore.Entities.NestedSets;
using System;

namespace OpenDataStorageCore.Entities.Aliases
{
    public class HierarchyObjectAlias : BaseAlias
    {
        public Guid HierarchyObjectId { get; set; }

        public HierarchyObject HierarchyObject { get; set; }
    }
}
