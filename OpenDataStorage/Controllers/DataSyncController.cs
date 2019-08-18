using OpenDataStorage.Helpers;
using System.Web.Mvc;

namespace OpenDataStorage.Controllers
{
    [Authorize(Roles = RolesHelper.DATA_SYNC_GROUP)]
    public class DataSyncController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}