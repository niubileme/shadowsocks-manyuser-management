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
    public class ProductController : BaseController
    {
        public ActionResult List()
        {
            return View();
        }

        public JsonResult GetList()
        {
            var offset = RequestHelper.GetInt("offset");
            var limit = RequestHelper.GetInt("limit");
            var key = RequestHelper.GetValue("search");
            int total;
            var list = ProductService.GetList(offset, limit, out total, key);
            return Json(new { rows = list, total = total }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public JsonResult AddPost()
        {
            var id = Guid.NewGuid().ToString("n");
            var name = RequestHelper.GetValue("name");
            var des = RequestHelper.GetValue("des");
            var traffic = RequestHelper.GetInt("traffic");
            var expirationday = RequestHelper.GetInt("expirationday");
            var price = RequestHelper.GetDecimal("price");
            var isrest = RequestHelper.GetInt("isrest");
            var sortnum = RequestHelper.GetInt("sortnum");
            if (string.IsNullOrEmpty(name) || traffic <= 0 || expirationday <= 0 || price <= 0)
            {
                return Json(new { result = false, info = "缺少必要参数。" }, JsonRequestBehavior.DenyGet);
            }
            var dto = new PruductDto()
            {
                Id = id,
                Name = name,
                Description = des,
                Traffic = traffic,
                ExpirationDay = expirationday,
                Price = price,
                IsRest = (sbyte)isrest,
                SortNum = sortnum
            };
            var result = ProductService.Add(dto);
            LogService.Info($"ProductService.Add >>> {result} --- {id}:{name}");
            return Json(new { result = result }, JsonRequestBehavior.DenyGet);
        }

        [HttpPost]
        public JsonResult UpdatePost()
        {
            var id = RequestHelper.GetValue("id");
            var name = RequestHelper.GetValue("name");
            var des = RequestHelper.GetValue("des");
            var traffic = RequestHelper.GetInt("traffic");
            var expirationday = RequestHelper.GetInt("expirationday");
            var price = RequestHelper.GetDecimal("price");
            var isrest = RequestHelper.GetInt("isrest");
            var sortnum = RequestHelper.GetInt("sortnum");
            if (string.IsNullOrEmpty(name) || traffic <= 0 || expirationday <= 0 || price <= 0)
            {
                return Json(new { result = false, info = "缺少必要参数" }, JsonRequestBehavior.DenyGet);
            }
            var dto = new PruductDto()
            {
                Id = id,
                Name = name,
                Description = des,
                Traffic = traffic,
                ExpirationDay = expirationday,
                Price = price,
                IsRest = (sbyte)isrest,
                SortNum = sortnum
            };
            var result = ProductService.Update(dto);
            LogService.Info($"ProductService.Update >>> {result} --- {id}:{name}");
            return Json(new { result = result }, JsonRequestBehavior.DenyGet);
        }

        [HttpPost]
        public JsonResult DeletePost()
        {
            var id = RequestHelper.GetValue("id");
            if (string.IsNullOrEmpty(id))
            {
                return Json(new { result = false }, JsonRequestBehavior.DenyGet);
            }
            var result = ProductService.Delete(id);
            LogService.Info($"ProductService.Delete >>> {result} --- {id}");
            return Json(new { result = result }, JsonRequestBehavior.DenyGet);
        }
    }
}