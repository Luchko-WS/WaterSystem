using OpenDataStorage.Core.Entities.NestedSets;
using System;

namespace OpenDataStorage.Core.Entities.Aliases
{
    public class HierarchyObjectAlias : BaseAlias
    {
        public Guid HierarchyObjectId { get; set; }

        public virtual HierarchyObject HierarchyObject { get; set; }
    }
}
