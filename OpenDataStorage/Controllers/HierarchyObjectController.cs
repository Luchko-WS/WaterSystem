using System;
using System.Web.Mvc;

namespace OpenDataStorage.Controllers
{
    public class HierarchyObjectController : Controller
    {
        [AllowAnonymous]
        public ActionResult HierarchyObjectTree()
        {
            return View();
        }
    }
}