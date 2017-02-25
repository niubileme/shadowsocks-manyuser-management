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
    public class OtherController : BaseController
    {
        /// <summary>
        /// 常见问题
        /// </summary>
        public ActionResult FAQ()
        {
            return View();
        }

        /// <summary>
        /// 公告通知
        /// </summary>
        public ActionResult Notice()
        {
            var list = NoticeService.GetList(20);
            ViewData["List"] = list;
            return View();
        }

        /// <summary>
        /// 公告详细
        /// </summary>
        public ActionResult NoticeInfo(string id)
        {
            if (string.IsNullOrEmpty(id))
                return RedirectToAction("Notice");
            var notice = NoticeService.Query(id);
            if (notice == null)
                return RedirectToAction("Notice");
            ViewData["Notice"] = notice;
            return View();
        }



        /// <summary>
        /// 推广计划
        /// </summary>
        public ActionResult Aff()
        {
            var user = UserCache.Cache.GetValue(CookieHelper.Email);
            var website= SettingCache.Cache.Get(SettingFlag.WebSiteUrl);
            var num = SettingCache.Cache.Get(SettingFlag.RebateNum);
            var customers = UserService.GetCustomers(user.Id);
            ViewData["Url"] = $"{website}?aff={user.AffCode}";
            ViewData["RebateNum"] = num;
            ViewData["Customers"] = customers;
            return View();
        }
    }
}