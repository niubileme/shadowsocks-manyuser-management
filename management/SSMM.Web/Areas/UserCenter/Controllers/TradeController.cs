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


        /// <summary>
        /// 订单详情
        /// </summary>
        public ActionResult Order(string id)
        {
            if (string.IsNullOrEmpty(id))
                return RedirectToAction("List", "Product");
            var product = ProductService.Query(id);
            if (product == null)
                return RedirectToAction("List", "Product");
            ViewData["Product"] = product;
            return View();
        }

        /// <summary>
        /// 优惠码
        /// </summary>
        [HttpPost]
        public JsonResult CheckPCode()
        {
            var pcode = RequestHelper.GetValue("pcode");
            if (string.IsNullOrEmpty(pcode))
                return Json(new { result = false, info = "优惠码不能为空！" }, JsonRequestBehavior.DenyGet);
            var info = "";
            var amount = 0;
            var result = true;
            return Json(new { result = result, info = info, amount = amount }, JsonRequestBehavior.DenyGet);
        }

        /// <summary>
        /// 支付方式
        /// </summary>
        public ActionResult PayMent()
        {
            var pid = RequestHelper.GetValue("pid");
            var pwd = RequestHelper.GetValue("pwd");
            var pcode = RequestHelper.GetValue("pcode");
            if (string.IsNullOrEmpty(pid)||string.IsNullOrEmpty(pwd))
                return RedirectToAction("List", "Product");
            ViewData["Pid"] = pid;
            ViewData["Pwd"] = pwd;
            ViewData["Pcode"] = pcode;
            return View();
        }


    }
}