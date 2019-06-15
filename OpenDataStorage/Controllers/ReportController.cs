using OpenDataStorage.Helpers;
using System.Web.Mvc;

namespace OpenDataStorage.Controllers
{
    [Authorize(Roles = RolesHelper.READ_GROUP)]
    public class ReportController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}