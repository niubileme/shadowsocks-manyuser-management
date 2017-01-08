using SSMM.Helper;
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
            //公告信息
            var notices = NoticeService.GetList(6);
            ViewData["Notices"] = notices;
            return View();
        }
    }
}