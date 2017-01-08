using SSMM.Helper;
using SSMM.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SSMM.Web.Areas.AdminCenter.Controllers
{
    public class UserController : BaseController
    {
        public ActionResult List()
        {
            return View();
        }

        public JsonResult GetList()
        {
            var offset = RequestHelper.GetInt("offset");
            var limit = RequestHelper.GetInt("limit");
            var key = RequestHelper.GetValue("search");
            int total;
            var list = UserService.GetList(offset, limit, out total, key);
            return Json(new { rows = list, total = total }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult UpdateStatus()
        {
            var id = RequestHelper.GetInt("id");
            var status = RequestHelper.GetInt("status");
            var result = UserService.UpdateStatus(id, status);
            return Json(new { result = result }, JsonRequestBehavior.DenyGet);
        }
    }
}