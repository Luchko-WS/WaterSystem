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
        public ActionResult Characteristic()
        {
            return PartialView();
        }

        public ActionResult CreateEditCharacteristic()
        {
            return PartialView();
        }

        public ActionResult CreateEditFolder()
        {
            return PartialView();
        }
    }
}