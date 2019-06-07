using OpenDataStorage.Resources;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Http;

namespace OpenDataStorage.API
{
    [AllowAnonymous]
    public class LanguageController : BaseApiController
    {
        [Route("api/Languages")]
        [HttpGet]
        public Dictionary<string, object> GetLexicon([FromUri]string lang)
        {
            var res = Lexicon.ResourceManager.GetResourceSet(new CultureInfo(lang), true, true);
            var d = from DictionaryEntry r in res select r;
            var dc = d.ToDictionary(k => k.Key.ToString(), v => v.Value);
            return dc;
        }

        [Route("UserLanguage")]
        public string GetUserLanguage()
        {
            return UserLanguage;
        }
    }
}