using OpenDataStorageCore.Entities.NestedSets;
using System.Linq;

namespace OpenDataStorage.ViewModels.CharacteristicViewModel
{
    public class CharacteristicViewModel : Characteristic
    {
        public string AliasesList
        {
            get
            {
                var aliasesNames = this.CharacteristicAliases.Select(a => a.Value).ToList();
                var res = string.Join(",", aliasesNames);
                return res;
            }
        }
    }
}