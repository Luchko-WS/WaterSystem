using OpenDataStorage.Core.Entities.NestedSets;

namespace OpenDataStorage.Core.Entities.CharacteristicValues
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
