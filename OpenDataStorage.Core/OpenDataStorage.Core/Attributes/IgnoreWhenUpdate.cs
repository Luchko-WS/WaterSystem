using System;

namespace OpenDataStorage.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = true)]
    public class IgnoreWhenUpdateAttribute : Attribute
    {
        public IgnoreWhenUpdateAttribute() { }
    }
}
