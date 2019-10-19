namespace OpenDataStorage.Core.Entities.NestedSets
{
    public class NestedSetsEntity : BaseEntity
    {
        public NestedSetsEntity() : base()
        { }

        public int Level { get; set; }

        public int LeftKey { get; set; }

        public int RightKey { get; set; }
    }
}
