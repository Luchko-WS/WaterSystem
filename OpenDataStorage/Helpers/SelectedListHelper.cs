using OpenDataStorage.Common;
using OpenDataStorageCore;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web.Mvc;

namespace OpenDataStorage.Helpers
{
    public static class SelectedListHelper
    {
        public static IEnumerable<SelectListItem> Countries
        {
            get
            {
                List<string> countries = new List<string>();
                foreach (CultureInfo ci in CultureInfo.GetCultures(CultureTypes.SpecificCultures))
                {
                    var ri = new RegionInfo(ci.Name);
                    countries.Add(ri.DisplayName);
                }
                countries = countries.Distinct().ToList();
                countries.Sort();

                return new SelectList(countries);
            }
        }

        public static IEnumerable<SelectListItem> AvailableLanguages
        {
            get
            {
                string selectedLanguage = LanguageManager.GetDefaultLanguage();
                string threadLanguage = Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName;
                if (LanguageManager.AvailableLanguages.Exists(l => l.LangCultureName == threadLanguage))
                {
                    selectedLanguage = threadLanguage;
                }
                return new SelectList(LanguageManager.AvailableLanguages, "LangCultureName", "LangFullName", selectedLanguage);
            }
        }

        public static IEnumerable<SelectListItem> CharacteristicTypes
        {
            get
            {
                var types = EnumHelper.GetValues<CharacteristicType>().Where(t => (int)t > 0).Select(t => new
                {
                    Text = t.ToString(),
                    Value = (int)t
                });
                return new SelectList(types, "Value", "Text");
            }
        }
    }
}