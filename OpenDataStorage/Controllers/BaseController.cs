using OpenDataStorage.Common;
using System;
using System.Web;
using System.Web.Mvc;

namespace OpenDataStorage.Controllers
{
    public class BaseController : Controller
    {
        //protected readonly ILogger _logger;

        public BaseController()
        {
            //_logger = ILogger();
        }

        //get cultureInfo and then get short language name (ex. ua, ru) from it.
        //after that set the value to ViewBag.Language in SetCurrentLanguage method
        protected override IAsyncResult BeginExecuteCore(AsyncCallback callback, object state)
        {
            string userLanguage;

            HttpCookie langCookie = Request.Cookies["culture"];
            if (langCookie != null)
            {
                userLanguage = langCookie.Value;
            }
            else
            {
                var userLang = Request.UserLanguages != null ? Request.UserLanguages[0] : string.Empty;
                userLanguage = string.IsNullOrEmpty(userLang) ? LanguageManager.GetDefaultLanguage() : userLang;
            }
            SetCurrentLanguage(userLanguage);

            return base.BeginExecuteCore(callback, state);
        }

        protected void SetCurrentLanguage(string language)
        {
            if (!string.IsNullOrWhiteSpace(language))
            {
                LanguageManager.SetLanguage(language);
                ViewBag.Culture = language;
            }
        }
    }
}