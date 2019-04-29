﻿using System.Web.Mvc;

namespace OpenDataStorage.Controllers
{
    public class ObjectTypeController : BaseController
    {
        [AllowAnonymous]
        public ActionResult Tree()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult Type()
        {
            return PartialView();
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
    }
}