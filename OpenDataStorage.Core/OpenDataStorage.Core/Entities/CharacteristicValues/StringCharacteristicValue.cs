using OpenDataStorage.Core.Entities.NestedSets;

namespace OpenDataStorage.Core.Entities.CharacteristicValues
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
