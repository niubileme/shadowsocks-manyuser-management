using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace SSMM.Helper
{
    public class CookieHelper
    {
        private const string XXX = "AUTUMNSS";
        private const string IL = "ISLOGIN";
        private const string IM = "ISMANAGER";
        private const string UN = "UN";
        private const string E = "EMAIL";

        public static string Domain
        {
            get
            {
                //var dom = ".xxx.com";
                var dom = "";
                return dom;
            }
        }

        public static string UserName
        {
            get { return GetValue(UN); }
            set { SetValue(UN, value); }
        }

        public static string Email
        {
            get { return GetValue(E); }
            set { SetValue(E, value); }
        }

        public static void SetLogin(string email, string username, int ismanager)
        {
            if (ismanager == 1)
            {
                var managerkey = string.Format("IsManager=true{0}{1}{2}", email, username, XXX);
                SetValue(IM, FormatHelper.GetMD5ByString(managerkey), 2);
            }
            var loginkey = string.Format("IsLogin=true{0}{1}{2}", email, username, XXX);
            SetValue(IL, FormatHelper.GetMD5ByString(loginkey), 2);
            SetValue(E, email);
            SetValue(UN, username);
        }

        public static void SetLoginOut()
        {
            var key = string.Format("IsLogin=false{0}{1}{2}", Email, UserName, XXX);
            SetValue(IL, FormatHelper.GetMD5ByString(key));
        }

        public static bool IsLogin()
        {
            var key = string.Format("IsLogin=true{0}{1}{2}", Email, UserName, XXX);
            var val = FormatHelper.GetMD5ByString(key);
            if (GetValue(IL) == val)
                return true;
            else
                return false;
        }

        public static bool IsManager()
        {
            var key = string.Format("IsManager=true{0}{1}{2}", Email, UserName, XXX);
            var val = FormatHelper.GetMD5ByString(key);
            if (GetValue(IM) == val)
                return true;
            else
                return false;
        }

        public static string GetValue(string keyName)
        {
            var cookie = HttpContext.Current.Request.Cookies[keyName];

            if (cookie != null)
            {
                return cookie.Value;
            }

            return "";
        }

        public static void SetValue(string keyName, string val)
        {
            SetValue(keyName, val, 6);
        }

        public static void SetValue(string keyName, string val, int expiresHour)
        {
            if (string.IsNullOrEmpty(val))
            {
                //设置过期，清空cookie
                var oldRes = HttpContext.Current.Response.Cookies[keyName];

                if (oldRes != null)
                {
                    oldRes.Value = val;
                    oldRes.Path = "/";
                    oldRes.Domain = Domain;
                    oldRes.Expires = DateTime.Today;
                }

                var oldReq = HttpContext.Current.Request.Cookies[keyName];

                if (oldReq != null)
                {
                    oldReq.Value = val;
                    oldReq.Path = "/";
                    oldReq.Domain = Domain;
                    oldReq.Expires = DateTime.Today;
                }

                return;
            }

            var cookieResponse = HttpContext.Current.Response.Cookies[keyName];

            if (cookieResponse != null)
            {
                cookieResponse.Value = val;
                cookieResponse.Path = "/";
                cookieResponse.Domain = Domain;
            }
            else
            {
                cookieResponse = new HttpCookie(keyName, val);

                cookieResponse.Path = "/";
                cookieResponse.Domain = Domain;

                HttpContext.Current.Response.AppendCookie(cookieResponse);
            }

            if (expiresHour > 0)
            {
                cookieResponse.Expires = DateTime.Now.AddHours(expiresHour);
            }

            var cookieRequest = HttpContext.Current.Request.Cookies[keyName];

            if (cookieRequest != null)
            {
                cookieRequest.Value = val;
            }
            else
            {
                cookieRequest = new HttpCookie(keyName, val);

                HttpContext.Current.Request.Cookies.Add(cookieRequest);
            }
        }
    }
}
