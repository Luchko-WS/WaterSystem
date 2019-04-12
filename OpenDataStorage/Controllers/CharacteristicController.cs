using System;
using System.Web.Mvc;

namespace OpenDataStorage.Controllers
{
    public class CharacteristicController : BaseController
    {
        [AllowAnonymous]
        public ActionResult CharacteristicTree()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult Characteristic(Guid id)
        {
            return View(id);
        }

        public ActionResult CreateEditCharacteristic()
        {
            return PartialView("CreateEditCharacteristic");
        }

        public ActionResult CreateEditFolder()
        {
            return PartialView("CreateEditFolder");
        }
    }
}