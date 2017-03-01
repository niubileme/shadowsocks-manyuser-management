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
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            var email = CookieHelper.Email;
            var user = UserService.Query(email);
            ViewData["User"] = user;
            //推广链接
            var website = SettingCache.Cache.Get(SettingFlag.WebSiteUrl);
            var affurl= $"{website}?aff={user.AffCode}";
            ViewData["AffUrl"] = affurl;
            var num = SettingCache.Cache.Get(SettingFlag.RebateNum);
            ViewData["RebateNum"] = num;
            //SS
            var ss = SSService.Query(user.Id);
            if (ss == null||!ss.status)
                //新用户 或者 服务已到期的用户 ss信息应为空
                ss = new Model.SSDto();
            ViewData["SS"] = ss;
            //公告信息
            var notices = NoticeService.GetList(6);
            ViewData["Notices"] = notices;
            return View();
        }
    }
}