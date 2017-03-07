
using SSMM.Helper;
using SSMM.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SSMM.Web.Areas.AdminCenter.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            var UserTotalCount = UserService.TotalCount();
            ViewData["UserTotalCount"] = UserTotalCount;
            //取当月
            DateTime now = DateTime.Now;
            DateTime d1 = new DateTime(now.Year, now.Month, 1);
            DateTime d2 = d1.AddMonths(1).AddDays(-1).AddHours(24);

            var CurrentMonthAmounts = OrderService.GetAmount(d1, d2);
            ViewData["CurrentMonthAmounts"] = CurrentMonthAmounts.ToString("0.00");
            //取上月
            var ld1 = d1.AddMonths(-1);
            var ld2 = d2.AddMonths(-1);
            var LastMonthAmounts = OrderService.GetAmount(ld1, ld2);
            ViewData["LastMonthAmounts"] = LastMonthAmounts.ToString("0.00");



            return View();
        }

        [HttpPost]
        public JsonResult GetCharts()
        {
            //var d1 = RequestHelper.GetDateTime("d1");
            //var d2 = RequestHelper.GetDateTime("d2");
            DateTime now = DateTime.Now;
            DateTime d1 = new DateTime(now.Year, now.Month, 1);
            DateTime d2 = d1.AddMonths(1).AddDays(-1).AddHours(24);
            //上上月
            d1 = d1.AddMonths(-2);
            d2 = d2.AddMonths(-2);
            var amountCharts = OrderService.GetAmountCharts(d1, d2);
            var typeCharts = OrderService.GetOrderPaymentTypeCharts(d1, d2);
            return Json(new { msg = true, amountCharts = amountCharts , typeCharts = typeCharts }, JsonRequestBehavior.DenyGet);
        }
    }
}