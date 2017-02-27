using SSMM.Model;
using SSMM.Services;
using SSMM.Web.Ext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SSMM.Web.Areas.UserCenter.Controllers
{
    [LoginAuthorize]
    public class BaseController : Controller
    {
        protected override void OnException(ExceptionContext filterContext)
        {
            base.OnException(filterContext);

            // 标记异常已处理
            filterContext.ExceptionHandled = true;

            var url = HttpUtility.UrlEncode(System.Web.HttpContext.Current.Request.Url.ToString());
            //log
            LogService.Error($"UserCenter.BaseController {url}", filterContext.Exception);

            // 跳转到错误页
            filterContext.Result = new RedirectResult("/Error/NotFound?returnurl=" + url);
        }


    }
}