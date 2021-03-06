﻿using OpenDataStorage.Helpers;
using System.Web.Mvc;

namespace OpenDataStorage.Controllers
{
    [Authorize(Roles = RolesHelper.DATA_MANAGEMENT_GROUP)]
    public class CharacteristicController : BaseController
    {
        //[AllowAnonymous]
        [Authorize(Roles = RolesHelper.READ_GROUP)]
        public ActionResult Tree()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult ReadCharacteristic()
        {
            return PartialView();
        }

        public ActionResult CreateEditCharacteristic()
        {
            return PartialView();
        }

        [AllowAnonymous]
        public ActionResult ReadFolder()
        {
            return PartialView();
        }

        public ActionResult CreateEditFolder()
        {
            return PartialView();
        }

        [AllowAnonymous]
        public ActionResult SelectCharacteristic()
        {
            return PartialView();
        }
    }
}