using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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

        [AllowAnonymous]
        public ActionResult HierarchyObject(Guid id)
        {
            return View(id);
        }

        public ActionResult CreateHierarchyObject()
        {
            return PartialView();
        }

        public ActionResult EditHierarchyObject()
        {
            return PartialView();
        }
    }
}