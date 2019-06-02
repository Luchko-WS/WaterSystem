using System;
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
        public ActionResult Details(Guid id)
        {
            ViewBag.Id = id;
            return View();
        }

        public ActionResult CreateEditObject()
        {
            return PartialView();
        }

        [AllowAnonymous]
        public ActionResult SelectObject()
        {
            return PartialView();
        }
    }
}