namespace OpenDataStorageCore
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
