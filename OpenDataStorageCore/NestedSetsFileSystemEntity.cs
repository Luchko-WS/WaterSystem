namespace OpenDataStorageCore
{
    public class NestedSetsFileSystemEntity: NestedSetsEntity
    {
        public NestedSetsFileSystemEntity() : base()
        { }

        public EntityType Type { get; set; }
    }

    public enum EntityType
    {
        Folder,
        File
    }
}
