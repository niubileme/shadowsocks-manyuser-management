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
    public class ServerNodeController : BaseController
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
            var list = ServerNodeService.GetList(offset, limit, out total, key);
            return Json(new { rows = list, total = total }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddPost()
        {
            var id = Guid.NewGuid().ToString("n");
            var name = RequestHelper.GetValue("servername");
            var ip = RequestHelper.GetValue("serverip");
            var cname = RequestHelper.GetValue("servercname");
            var des = RequestHelper.GetValue("serverdes");
            var status = 1;
            var sortnum = RequestHelper.GetInt("serversortnum");
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(ip))
            {
                return RedirectToAction("Add");
            }
            var dto = new ServerNodeDto()
            {
                Id = id,
                Name = name,
                IP = ip,
                CNAME = cname,
                Description = des,
                Status = (sbyte)status,
                SortNum = sortnum
            };
            var result = ServerNodeService.Add(dto);
            LogService.Info($"ServerNodeService.Add >>> {result} --- {id}:{ip}");
            return RedirectToAction("List");
        }

        [HttpPost]
        public JsonResult UpdatePost()
        {
            var id = RequestHelper.GetValue("id");
            var name = RequestHelper.GetValue("name");
            var ip = RequestHelper.GetValue("ip");
            var cname = RequestHelper.GetValue("cname");
            var des = RequestHelper.GetValue("des");
            var status = RequestHelper.GetInt("status");
            var sortnum = RequestHelper.GetInt("sortnum");
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(ip))
            {
                return Json(new { result = false, info = "缺少必要参数" }, JsonRequestBehavior.DenyGet);
            }
            var dto = new ServerNodeDto()
            {
                Id = id,
                Name = name,
                IP = ip,
                CNAME = cname,
                Description = des,
                Status = (sbyte)status,
                SortNum = sortnum
            };
            var result = ServerNodeService.Update(dto);
            LogService.Info($"ServerNodeService.Update >>> {result} --- {id}:{ip}");
            return Json(new { result = result }, JsonRequestBehavior.DenyGet);
        }

        [HttpPost]
        public JsonResult DeletePost()
        {
            var id = RequestHelper.GetValue("id");
            if (string.IsNullOrEmpty(id))
            {
                return Json(new { result = false}, JsonRequestBehavior.DenyGet);
            }
            var result = ServerNodeService.Delete(id);
            LogService.Info($"ServerNodeService.Delete >>> {result} --- {id}");
            return Json(new { result = result }, JsonRequestBehavior.DenyGet);
        }
    }
}