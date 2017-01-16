using SSMM.Helper;
using SSMM.Model;
using SSMM.Services;
using SSMM.Web.Ext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace SSMM.Web.Areas.UserCenter.Controllers
{
    public class AccountController : Controller
    {
        /// <summary>
        /// 基本信息
        /// </summary>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult Index()
        {
            return View();
        }

        [LoginAuthorize]
        public ActionResult RestPassword()
        {
            return View();
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        [HttpPost]
        public JsonResult PostRestPassword()
        {
            var email = CookieHelper.Email;
            var oldpwd = RequestHelper.GetValue("oldpwd");
            var newpwd = RequestHelper.GetValue("newpwd");
            if (string.IsNullOrEmpty(oldpwd) || string.IsNullOrEmpty(newpwd))
                return Json(new { result = false, info = "密码不能为空！" }, JsonRequestBehavior.DenyGet);
            if (newpwd.Length < 6)
                return Json(new { result = false, info = "新密码不能少于6位！" }, JsonRequestBehavior.DenyGet);
            var info = "";
            var result = UserService.RestPassword(email, oldpwd, newpwd, out info);
            return Json(new { result = result, info = info }, JsonRequestBehavior.DenyGet);
        }

        /// <summary>
        /// 登录
        /// </summary>
        public ActionResult Login()
        {
            var urlpath = RequestHelper.GetValue("returnurl");
            ViewData["urlpath"] = string.IsNullOrEmpty(urlpath) ? "/UserCenter/Home/Index" : urlpath;
            return View();
        }

        [HttpPost]
        public JsonResult LoginPost()
        {
            var email = RequestHelper.GetValue("email");
            var password = RequestHelper.GetValue("pwd");
            var url = RequestHelper.GetValue("url");
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                return Json(new { result = false, info = "Email、密码不能为空！" }, JsonRequestBehavior.DenyGet);
            }
            if (string.IsNullOrEmpty(url))
            {
                url = "/UserCenter/Home/Index";
            }
            else
            {
                var ticks = DateTime.Now.Ticks;
                url += Regex.IsMatch(HttpUtility.UrlDecode(url), "\\?") ? "&" : "?" + "t=" + ticks;
            }
            //验证 登录
            UserDto currentuser = null;
            var info = "";
            var result = UserService.Login(email, password, out info, out currentuser);
            if (!result)
            {
                return Json(new { result = result, info = info }, JsonRequestBehavior.DenyGet);
            }
            CookieHelper.SetLogin(currentuser.Email, currentuser.UserName, currentuser.IsManager);
            return Json(new { result = result, url = url }, JsonRequestBehavior.DenyGet);
        }

        /// <summary>
        /// 退出登录
        /// </summary>
        /// <returns></returns>
        public RedirectResult LoginOut()
        {
            Session.Clear();
            CookieHelper.SetLoginOut();
            return Redirect("/");
        }

        /// <summary>
        /// 注册
        /// </summary>
        public ActionResult Register()
        {
            var urlpath = RequestHelper.GetValue("returnurl");
            ViewData["urlpath"] = string.IsNullOrEmpty(urlpath) ? "/UserCenter/Home/Index" : urlpath;
            return View();
        }


        [HttpPost]
        public JsonResult RegisterPost()
        {
            var username = RequestHelper.GetValue("uname");
            var password = RequestHelper.GetValue("pwd");
            var vcode = RequestHelper.GetValue("vcode");
            var email = RequestHelper.GetValue("email");
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(vcode) || string.IsNullOrEmpty(email))
            {
                return Json(new { result = false, info = "字段不能为空！" }, JsonRequestBehavior.DenyGet);
            }
            if (username.Length < 4)
            {
                return Json(new { result = false, info = "用户名不能少于4个字符！" }, JsonRequestBehavior.DenyGet);
            }
            if (password.Length < 6)
            {
                return Json(new { result = false, info = "密码不能少于6位！" }, JsonRequestBehavior.DenyGet);
            }
            var serverCode = SessionHelper.GetValue("ValidateCode");
            if (serverCode == null || serverCode.ToString() != vcode)
            {
                return Json(new { result = false, info = "验证码错误！" }, JsonRequestBehavior.DenyGet);
            }
            //清除会话
            Session.Remove("ValidateCode");
            var info = "";
            //注册
            var result = UserService.Register(username, email, password, out info);
            if (!result)
            {
                return Json(new { result = false, info = info }, JsonRequestBehavior.DenyGet);
            }
            return Json(new { result = true }, JsonRequestBehavior.DenyGet);
        }

        /// <summary>
        /// 获取验证码
        /// </summary>
        public ActionResult GetValidateCode()
        {
            string code = ValidateCode.CreateValidateCode(4);
            SessionHelper.SetValue("ValidateCode", code);
            byte[] bytes = ValidateCode.CreateValidateGraphic(code);
            return File(bytes, @"image/jpeg");
        }
    }
}