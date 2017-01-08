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
    }
}