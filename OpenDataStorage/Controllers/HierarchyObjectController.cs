using System.Web.Mvc;

namespace OpenDataStorage.Controllers
{
    public class HierarchyObjectController : BaseController
    {
        [AllowAnonymous]
        public ActionResult Tree()
        {
            return View();
        }

        public ActionResult CreateEditObject()
        {
            return PartialView();
        }
    }
}