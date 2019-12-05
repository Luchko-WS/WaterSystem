using Newtonsoft.Json;
using OpenDataStorage.Core.Entities.CharacteristicValues;
using System;
using System.Collections.Generic;

namespace SyncOpenDateServices.SacmigFormat
{
    public class SacmigFileData
    {
        [JsonProperty]
        private readonly Dictionary<string, List<NumberCharacteristicValue>> _characteristicValues;

        public SacmigFileData()
        {
            _characteristicValues = new Dictionary<string, List<NumberCharacteristicValue>>();
        }

        public string ObjectName { get; set; }

        public string SubjectOfMonitoring { get; set; }

        public void AddCharacteristicValue(string characteristicName, double value, DateTime startDate, DateTime? endDate = null)
        {
            if (!_characteristicValues.ContainsKey(characteristicName))
            {
                _characteristicValues[characteristicName] = new List<NumberCharacteristicValue>();
            }

            var isInterval = endDate.HasValue;
            var entity = new NumberCharacteristicValue
            {
                IsTimeIntervalValue = isInterval,
                CreationDate = startDate,
                EndCreationDate = isInterval ? endDate : null,
                Value = value,
                SubjectOfMonitoring = SubjectOfMonitoring
            };
            _characteristicValues[characteristicName].Add(entity);
        }

        public IEnumerable<NumberCharacteristicValue> GetCharacteristicValues(string characteristicName)
        {
            return _characteristicValues.ContainsKey(characteristicName) ?
                _characteristicValues[characteristicName] : null;
        }

        public IEnumerable<string> GetCharacteristicsNames()
        {
            return _characteristicValues.Keys;
        }
    }
}
