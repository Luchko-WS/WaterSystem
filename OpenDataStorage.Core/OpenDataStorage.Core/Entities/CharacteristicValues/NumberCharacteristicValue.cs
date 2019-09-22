using OpenDataStorageCore.Entities.NestedSets;

namespace OpenDataStorageCore.Entities.CharacteristicValues
{
    public class NumberCharacteristicValue : BaseCharacteristicValue
    {
        public NumberCharacteristicValue() :base()
        {
            ValueType = CharacteristicType.Number;
        }

        public double Value { get; set; }
    }
}
