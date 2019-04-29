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

        [AllowAnonymous]
        public ActionResult ReadObject()
        {
            return PartialView();
        }

        public ActionResult CreateEditObject()
        {
            return PartialView();
        }
    }
}