using SSMM.Cache;
using SSMM.Helper;
using SSMM.Model;
using SSMM.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SSMM.Web.Areas.AdminCenter.Controllers
{
    public class SettingController : BaseController
    {

        public ActionResult WebSite()
        {
            ViewBag.WebSiteTitle = SettingCache.Cache.Get(SettingFlag.WebSiteTitle);
            ViewBag.WebSiteKeywords = SettingCache.Cache.Get(SettingFlag.WebSiteKeywords);
            ViewBag.WebSiteDescription = SettingCache.Cache.Get(SettingFlag.WebSiteDescription);
            ViewBag.StatisticalCode = FormatHelper.HtmlDecode(SettingCache.Cache.Get(SettingFlag.StatisticalCode));
            return View();
        }

        [HttpPost]
        public JsonResult WebSitePost()
        {
            var title = RequestHelper.GetValue("title");
            var keywords = RequestHelper.GetValue("keywords");
            var des = RequestHelper.GetValue("des");
            var code = RequestHelper.GetValue("code");
            if (string.IsNullOrEmpty(title))
            {
                return Json(new { result = false, info = "标题不能为空！" }, JsonRequestBehavior.DenyGet);
            }
            var result = SettingService.WebSite(title, keywords, des, code);
            LogService.Info($"修改WebSite >>> {result} --- {title}:{keywords}:{des}");
            return Json(new { result = result }, JsonRequestBehavior.DenyGet);
        }



        public ActionResult Basic()
        {
            ViewBag.AlipayAccount = SettingCache.Cache.Get(SettingFlag.AlipayAccount);
            ViewBag.AlipaySearchApi = SettingCache.Cache.Get(SettingFlag.AlipaySearchApi);
            return View();
        }

        [HttpPost]
        public JsonResult BasicPost()
        {
            var alipayaccount = RequestHelper.GetValue("alipayaccount");
            var alipaysearchapi = RequestHelper.GetValue("alipaysearchapi");
            if (string.IsNullOrEmpty(alipayaccount) || string.IsNullOrEmpty(alipaysearchapi))
            {
                return Json(new { result = false, info = "该参数不能为空！" }, JsonRequestBehavior.DenyGet);
            }
            var result = SettingService.Basic(alipayaccount, alipaysearchapi);
            LogService.Info($"修改Basic >>> {result} --- {alipayaccount}:{alipaysearchapi}");
            return Json(new { result = result }, JsonRequestBehavior.DenyGet);
        }

    }
}