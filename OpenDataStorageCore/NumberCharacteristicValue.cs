namespace OpenDataStorageCore
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
