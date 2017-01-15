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
          
            return View();
        }
    }
}