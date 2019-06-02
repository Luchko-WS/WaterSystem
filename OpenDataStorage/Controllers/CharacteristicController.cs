using System.Web.Mvc;

namespace OpenDataStorage.Controllers
{
    public class CharacteristicController : BaseController
    {
        [AllowAnonymous]
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