﻿using SSMM.Services;
using SSMM.Web.Ext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SSMM.Web.Areas.AdminCenter.Controllers
{
    [ManagerAuthorize]
    public class BaseController : Controller
    {

        protected override void OnException(ExceptionContext filterContext)
        {
            base.OnException(filterContext);

            // 标记异常已处理
            filterContext.ExceptionHandled = true;

            //log
            LogService.Error("AdminCenter.BaseController", filterContext.Exception);
        }
    }
}