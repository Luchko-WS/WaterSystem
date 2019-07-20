using OpenDataStorage.Helpers;
using System;
using System.Web.Mvc;

namespace OpenDataStorage.Controllers
{
    [Authorize(Roles = RolesHelper.DATA_MANAGEMENT_GROUP)]
    public class HierarchyObjectController : BaseController
    {
        [Authorize(Roles = RolesHelper.READ_GROUP)]
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