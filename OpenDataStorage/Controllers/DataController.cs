using OpenDataStorage.Helpers;
using System.Web.Mvc;

namespace OpenDataStorage.Controllers
{
    [Authorize(Roles = RolesHelper.DATA_MANAGEMENT_GROUP)]
    public class DataController : BaseController
    { 
        public ActionResult CreateEditNumber()
        {
            return PartialView();
        }

        public ActionResult CreateEditString()
        {
            return PartialView();
        }
    }
}