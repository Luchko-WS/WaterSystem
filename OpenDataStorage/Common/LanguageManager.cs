using OpenDataStorage.Resources;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;

namespace OpenDataStorage.Common
{
    public class Language
    {
        public string LangFullName { get; set; }

        public string LangCultureName { get; set; }
    }

    public class LanguageManager
    {
        public static List<Language> AvailableLanguages
        {
            get
            {
                return new List<Language>
                {
                    new Language { LangFullName = "English", LangCultureName = "en" },
                    new Language { LangFullName = "Українська", LangCultureName = "uk" },
                    new Language { LangFullName = "Русский", LangCultureName = "ru" }
                };
            }
        }

        public static bool IsLanguageAvailable(string Language)
        {
            return AvailableLanguages.Exists(l => l.LangCultureName.Equals(Language));
        }

        public static string GetDefaultLanguage()
        {
            return AvailableLanguages[1].LangCultureName;
        }

        public static void SetLanguage(string lang)
        {
            try
            {
                if (!IsLanguageAvailable(lang)) lang = GetDefaultLanguage();
                CultureInfo cultureInfo = new CultureInfo(lang);
                Thread.CurrentThread.CurrentUICulture = cultureInfo;
                Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(cultureInfo.Name);
                Lexicon.Culture = cultureInfo;

                HttpCookie langCookie = new HttpCookie("culture", lang)
                {
                    Expires = DateTime.Now.AddYears(1)
                };
                HttpContext.Current.Response.Cookies.Remove("culture");
                HttpContext.Current.Response.Cookies.Add(langCookie);
            }
            catch (Exception)
            {
                //add log
                throw;
            }
        }
    }
}