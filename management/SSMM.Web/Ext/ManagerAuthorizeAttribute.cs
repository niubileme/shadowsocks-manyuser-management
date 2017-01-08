using SSMM.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SSMM.Web.Ext
{
    /// <summary>
    /// 管理验证
    /// </summary>
    public class ManagerAuthorizeAttribute: AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (!CookieHelper.IsManager())
            {
                //filterContext.Result = new RedirectResult("/UserCenter/Account/Login?returnurl=" + (HttpUtility.UrlEncode(System.Web.HttpContext.Current.Request.Url.ToString())));
                filterContext.Result = new RedirectResult("/UserCenter/Home/Index");
            }
        }
    }
}