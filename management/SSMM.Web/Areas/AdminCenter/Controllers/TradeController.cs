using SSMM.Helper;
using SSMM.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SSMM.Web.Areas.AdminCenter.Controllers
{
    public class TradeController : BaseController
    {
        public ActionResult OrderList()
        {
            return View();
        }

        public JsonResult GetOrderList()
        {
            var offset = RequestHelper.GetInt("offset");
            var limit = RequestHelper.GetInt("limit");
            var key = RequestHelper.GetValue("search");
            int total;
            var list = TradeService.GetOrderList(offset, limit, out total, key);
            return Json(new { rows = list, total = total }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult BillList()
        {
            return View();
        }

        public JsonResult GetBillList()
        {
            var offset = RequestHelper.GetInt("offset");
            var limit = RequestHelper.GetInt("limit");
            var key = RequestHelper.GetValue("search");
            int total;
            var list = TradeService.GetBillList(offset, limit, out total, key);
            return Json(new { rows = list, total = total }, JsonRequestBehavior.AllowGet);
        }
    }
}