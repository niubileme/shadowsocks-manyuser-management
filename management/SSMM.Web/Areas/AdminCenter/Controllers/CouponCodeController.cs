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
    public class CouponCodeController : BaseController
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
            var list = CouponCodeService.GetList(offset, limit, out total, key);
            return Json(new { rows = list, total = total }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddPost()
        {
            var des = RequestHelper.GetValue("des");
            var amount = RequestHelper.GetDecimal("amount");
            var expirationtime = RequestHelper.GetInt("expirationtime");
            if (amount <= 0 || expirationtime <= 0)
            {
                return RedirectToAction("Add");
            }
            var dto = new CouponCodeDto()
            {
                Code = FormatHelper.GetCouponCode(),
                Des = des,
                Amount = amount,
                CreateTime = DateTime.Now,
                ExpirationTime = DateTime.Now.AddDays(expirationtime),
                MaxCount = 0,
                Status = 1
            };
            var result = CouponCodeService.Add(dto);
            LogService.Info($"CouponCodeService.Add >>> {result} --- {dto.Code}");
            return RedirectToAction("List");
        }

        [HttpPost]
        public JsonResult UpdatePost()
        {
            var id = RequestHelper.GetInt("id");
            var status = RequestHelper.GetInt("status");

            if (id < 0 || status > 1 || status < 0)
            {
                return Json(new { result = false, info = "参数不正确" }, JsonRequestBehavior.DenyGet);
            }

            var result = CouponCodeService.Update(id, status);
            LogService.Info($"CouponCodeService.Update >>> {result} --- {id}:{status}");
            return Json(new { result = result }, JsonRequestBehavior.DenyGet);
        }
    }
}