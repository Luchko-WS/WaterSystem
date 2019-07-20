﻿using OpenDataStorageCore.Entities.NestedSets;
using System.Linq;

namespace OpenDataStorage.ViewModels.HierarchyObjectViewModels
{
    public class HierarchyObjectViewModel : HierarchyObject
    {
        public string AliasesList
        {
            get
            {
                var aliasesNames = this.HierarchyObjectAliases.Select(a => a.Value).ToList();
                var res = string.Join(",", aliasesNames);
                return res;
            }
        }
    }
}