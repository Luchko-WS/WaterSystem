using OpenDataStorage.Helpers;
using System;
using System.Web.Mvc;

namespace OpenDataStorage.Controllers
{
    [Authorize(Roles = RolesHelper.DATA_SYNC_GROUP)]
    public class DataSyncController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Authorize(Roles = RolesHelper.DATA_MANAGEMENT_GROUP)]
        public ActionResult ImportDataFromFile(Guid? id)
        {
            ViewBag.Id = id;
            return View();
        }
    }
}