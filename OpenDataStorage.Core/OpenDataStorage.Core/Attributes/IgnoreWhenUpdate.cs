using System;

namespace OpenDataStorageCore.Attributes
{
    [AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = true)]
    public class IgnoreWhenUpdateAttribute : Attribute
    {
        public IgnoreWhenUpdateAttribute() { }
    }
}
