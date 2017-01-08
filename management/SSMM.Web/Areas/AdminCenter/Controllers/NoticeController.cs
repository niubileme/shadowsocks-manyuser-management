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
    public class NoticeController : BaseController
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
            var list = NoticeService.GetList(offset, limit, out total, key);
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
            var title = RequestHelper.GetValue("title");
            var contents = RequestHelper.GetValue("contents");
            if (string.IsNullOrEmpty(title) || string.IsNullOrEmpty(contents))
            {
                return Json(new { result = false, info = "缺少标题，内容。" }, JsonRequestBehavior.DenyGet);
            }
            var dto = new NoticeDto()
            {
                Id = id,
                CreateTime = DateTime.Now,
                Title = title,
                Contents = contents,
                IsDelete = 0
            };
            var result = NoticeService.Add(dto);
            LogService.Info($"NoticeService.Add >>> {result} --- {id}:{title}");
            return Json(new { result = result }, JsonRequestBehavior.DenyGet);
        }

        [HttpPost]
        public JsonResult UpdatePost()
        {
            var id = RequestHelper.GetValue("id");
            var title = RequestHelper.GetValue("title");
            var contents = RequestHelper.GetValue("contents");
            if (string.IsNullOrEmpty(title) || string.IsNullOrEmpty(contents))
            {
                return Json(new { result = false, info = "缺少必要参数" }, JsonRequestBehavior.DenyGet);
            }
            var dto = new NoticeDto()
            {
                Id = id,
                CreateTime = DateTime.Now,
                Title = title,
                Contents = contents,
                IsDelete = 0
            };
            var result = NoticeService.Update(dto);
            LogService.Info($"NoticeService.Update >>> {result} --- {id}:{title}");
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
            var result = NoticeService.Delete(id);
            LogService.Info($"NoticeService.Delete >>> {result} --- {id}");
            return Json(new { result = result }, JsonRequestBehavior.DenyGet);
        }
    }
}