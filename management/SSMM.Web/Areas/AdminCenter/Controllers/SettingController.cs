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
            ViewBag.WebSiteUrl = SettingCache.Cache.Get(SettingFlag.WebSiteUrl);
            ViewBag.WebSiteTitle = SettingCache.Cache.Get(SettingFlag.WebSiteTitle);
            ViewBag.WebSiteKeywords = SettingCache.Cache.Get(SettingFlag.WebSiteKeywords);
            ViewBag.WebSiteDescription = SettingCache.Cache.Get(SettingFlag.WebSiteDescription);
            ViewBag.StatisticalCode = FormatHelper.HtmlDecode(SettingCache.Cache.Get(SettingFlag.StatisticalCode));
            return View();
        }

        [HttpPost]
        public JsonResult WebSitePost()
        {
            var url = RequestHelper.GetValue("url");
            var title = RequestHelper.GetValue("title");
            var keywords = RequestHelper.GetValue("keywords");
            var des = RequestHelper.GetValue("des");
            var code = RequestHelper.GetValue("code");
            if (string.IsNullOrEmpty(title))
            {
                return Json(new { result = false, info = "网站标题不能为空！" }, JsonRequestBehavior.DenyGet);
            }
            var result = SettingService.WebSite(url,title, keywords, des, code);
            LogService.Info($"修改WebSite >>> {result} --- {title}:{keywords}:{des}");
            return Json(new { result = result }, JsonRequestBehavior.DenyGet);
        }



        public ActionResult Basic()
        {
            ViewBag.RebateNum = SettingCache.Cache.Get(SettingFlag.RebateNum);
            ViewBag.AlipayAccount = SettingCache.Cache.Get(SettingFlag.AlipayAccount);
            ViewBag.AlipayRemarkSearchApi = SettingCache.Cache.Get(SettingFlag.AlipayRemarkSearchApi);
            var portrange = SettingCache.Cache.Get(SettingFlag.SSPortRange).Split('&');
            ViewBag.SSPortMin = portrange[0];
            ViewBag.SSPortMax = portrange[1];
            ViewBag.WebSiteInfo = FormatHelper.HtmlDecode(SettingCache.Cache.Get(SettingFlag.WebSiteInfo));
            return View();
        }

        [HttpPost]
        public JsonResult BasicPost()
        {
            var alipayaccount = RequestHelper.GetValue("alipayaccount");
            var alipayremarksearchapi = RequestHelper.GetValue("alipayremarksearchapi");
            var portmin = RequestHelper.GetInt("portmin");
            var portmax = RequestHelper.GetInt("portmax");
            var rebatenum = RequestHelper.GetInt("rebatenum");
            var websiteinfo = RequestHelper.GetValue("websiteinfo");
            if (string.IsNullOrEmpty(alipayaccount) || string.IsNullOrEmpty(alipayremarksearchapi) || portmin <= 0 || portmax <= 0 || portmax > 65535)
            {
                return Json(new { result = false, info = "该参数不能为空！" }, JsonRequestBehavior.DenyGet);
            }
            var range = $"{portmin}&{portmax}";
            var result = SettingService.Basic(alipayaccount, alipayremarksearchapi, range, rebatenum, websiteinfo);
            LogService.Info($"修改Basic >>> {result} --- {alipayaccount}:{alipayremarksearchapi}");
            return Json(new { result = result }, JsonRequestBehavior.DenyGet);
        }

    }
}