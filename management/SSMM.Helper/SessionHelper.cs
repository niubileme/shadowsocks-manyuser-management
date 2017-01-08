using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace SSMM.Helper
{
    public class SessionHelper
    {
        public static bool IsLogin()
        {
            //var account = GetValue("User") as AccountModel;
            //if (account == null)
            //{
            //    return false;
            //}
            return true;
        }

        public static bool CurrentUser()
        {
            //var user = GetValue("CurrentUser") as AccountModel;
            //if (account == null)
            //{
            //    return false;
            //}
            return true;
        }

        public static object GetValue(string keyName)
        {
            return HttpContext.Current.Session[keyName];
        }

        public static void SetValue(string keyName, object val)
        {
            if (val == null || string.IsNullOrEmpty(val.ToString()))
            {
                HttpContext.Current.Session.Remove(keyName);
            }
            else
            {
                HttpContext.Current.Session[keyName] = val;
            }
        }

    }
}
