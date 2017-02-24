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
    public class ProductController : BaseController
    {
        /// <summary>
        /// 购买服务
        /// </summary>
        public ActionResult List()
        {
            var products = ProductService.GetAll();
            ViewData["Products"] = products;
            return View();
        }

        /// <summary>
        /// 我的服务
        /// </summary>
        public ActionResult My()
        {
            var email = CookieHelper.Email;
            var user = UserCache.Cache.GetValue(email);
            var my = ProductService.MyInfo(user.Id);
            if (my == null || !my.status)
                //服务不存在 或 已过期
                my = new MyProductDto();
            var nodes = ServerNodeService.GetAll(my);
            ViewData["My"] = my;
            ViewData["Nodes"] = nodes;
            return View();
        }
    }
}