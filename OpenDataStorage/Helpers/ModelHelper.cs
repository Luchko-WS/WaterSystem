using OpenDataStorage.Common;
using OpenDataStorageCore.Entities.CharacteristicValues;

namespace OpenDataStorage.Helpers
{
    public static class ModelHelper
    {
        public static BaseCharacteristicValue ConvertToViewModelIfExists(this BaseCharacteristicValue item)
        {
            if (item is NumberCharacteristicValue)
            {
                return Mapper.CreateInstanceAndMapProperties<NumberCharacteristicValue>(item);
            }
            if (item is StringCharacteristicValue)
            {
                return Mapper.CreateInstanceAndMapProperties<StringCharacteristicValue>(item);
            }
            return item;
        }
    }
}