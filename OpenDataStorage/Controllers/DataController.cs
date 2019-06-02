using System.Web.Mvc;

namespace OpenDataStorage.Controllers
{
    public class DataController : BaseController
    { 
        public ActionResult CreateEditNumber()
        {
            return PartialView();
        }

        public ActionResult CreateEditString()
        {
            return PartialView();
        }
    }
}