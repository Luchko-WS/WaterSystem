using OpenDataStorage.Helpers;
using System;
using System.Web.Mvc;

namespace OpenDataStorage.Controllers
{
    [Authorize(Roles = RolesHelper.DATA_MANAGEMENT_GROUP)]
    public class AliasController : BaseController
    {
        [HttpGet]
        public ActionResult CharacteristicAliases(Guid id)
        {
            ViewBag.EntityType = "characteristic";
            ViewBag.Id = id;
            return View("Index");
        }

        [HttpGet]
        public ActionResult ObjectAliases(Guid id)
        {
            ViewBag.EntityType = "object";
            ViewBag.Id = id;
            return View("Index");
        }
    }
}