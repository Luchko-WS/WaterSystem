using OpenDataStorageCore.Entities.NestedSets;

namespace OpenDataStorageCore.Entities.CharacteristicValues
{
    public class StringCharacteristicValue : BaseCharacteristicValue
    {
        public StringCharacteristicValue() : base()
        {
            ValueType = CharacteristicType.String;
        }

        public string Value { get; set; }
    }
}
