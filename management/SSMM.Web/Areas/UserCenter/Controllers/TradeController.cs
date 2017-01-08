using SSMM.Cache;
using SSMM.Helper;
using SSMM.Model;
using SSMM.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SSMM.Web.Areas.UserCenter.Controllers
{
    public class TradeController : BaseController
    {
        /// <summary>
        /// 充值
        /// </summary>
        public ActionResult Recharge()
        {
            var AlipayAccount = SettingCache.Cache.Get(SettingFlag.AlipayAccount);
            ViewBag.AlipayAccount = AlipayAccount;
            return View();
        }

        [HttpPost]
        public ActionResult CheckTradeNo()
        {
            var email = CookieHelper.Email;
            var tradeno = RequestHelper.GetValue("tradeno");
            if (string.IsNullOrEmpty(tradeno))
            {
                return Json(new { result = false, info = "交易号不能为空！" }, JsonRequestBehavior.DenyGet);
            }
            var info = "";
            var result = TradeService.AlipayTransfer(email, tradeno, out info);
            return Json(new { result = result, info = info }, JsonRequestBehavior.DenyGet);
        }


        public ActionResult Order()
        {
            return View();
        }

    }
}