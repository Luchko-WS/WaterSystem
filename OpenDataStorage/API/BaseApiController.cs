using Microsoft.AspNet.Identity.Owin;
using OpenDataStorage.Common.DbContext;
using OpenDataStorage.Resources;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Resources;
using System.Security.Claims;
using System.Web;
using System.Web.Http;

namespace OpenDataStorage.API
{
    [Authorize]
    public class BaseApiController : ApiController
    {
        private string _language;
        private ResourceSet _lexicon;

        protected readonly IApplicationDbContext _dbContext;

        protected BaseApiController()
        {
            this._dbContext = new ApplicationDbContext();
        }

        protected ApplicationUserManager UserManager
        {
            get
            {
                return HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
        }

        public string UserLanguage
        {
            get
            {
                if (this._language == null)
                {
                    _language = (RequestContext.Principal.Identity as ClaimsIdentity)?.Claims.FirstOrDefault(c => c.Type == "Lang")?.Value;
                    if (this._language == null)
                    {
                        var langCookie = Request.Headers.GetCookies("culture").FirstOrDefault();
                        if (langCookie != null)
                        {
                            _language = langCookie["culture"].Value;
                        }
                    }
                }
                return this._language ?? "uk";
            }
        }

        public ResourceSet UserLexicon
        {
            get
            {
                if (_lexicon == null)
                {
                    _lexicon = Lexicon.ResourceManager.GetResourceSet(new CultureInfo(UserLanguage), true, true);
                }

                return _lexicon;
            }
        }
    }
}
