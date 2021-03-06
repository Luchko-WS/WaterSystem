﻿using OpenDataStorage.Helpers;
using System.Web.Mvc;

namespace OpenDataStorage.Controllers
{
    [Authorize(Roles = RolesHelper.DATA_MANAGEMENT_GROUP)]
    public class ObjectTypeController : BaseController
    {
        [Authorize(Roles = RolesHelper.READ_GROUP)]
        public ActionResult Tree()
        {
            return View();
        }

        public ActionResult CreateEditType()
        {
            return PartialView();
        }

        [AllowAnonymous]
        public ActionResult ReadType()
        {
            return PartialView();
        }

        public ActionResult CreateEditFolder()
        {
            return PartialView();
        }

        [AllowAnonymous]
        public ActionResult ReadFolder()
        {
            return PartialView();
        }

        [AllowAnonymous]
        public ActionResult SelectType()
        {
            return PartialView();
        }
    }
}