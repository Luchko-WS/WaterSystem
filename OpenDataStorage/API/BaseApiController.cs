using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using OpenDataStorage.Common;
using OpenDataStorage.Common.Attributes;
using OpenDataStorage.Core.DataAccessLayer.DbContext;
using OpenDataStorage.Resources;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Resources;
using System.Web;
using System.Web.Http;

namespace OpenDataStorage.API
{
    [WebApiAuthorize]
    public abstract class BaseApiController : ApiController
    {
        private string _language;
        private ResourceSet _lexicon;

        protected readonly IOpenDataStorageDbContext _dbContext;

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
                    var langCookie = Request.Headers.GetCookies("culture").FirstOrDefault();
                    if (langCookie != null)
                    {
                        _language = langCookie["culture"].Value;
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

        protected RoleManager<IdentityRole> RoleManager
        {
            get
            {
                var context = HttpContext.Current.GetOwinContext().Get<ApplicationDbContext>();
                var roleStore = new RoleStore<IdentityRole>(context);
                var roleManager = new RoleManager<IdentityRole>(roleStore);
                return roleManager;
            }
        }
    }
}
